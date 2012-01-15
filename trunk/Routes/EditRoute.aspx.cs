using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MTBScout;
using System.IO;
using MTBScout.Entities;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web.Security;

public partial class Routes_EditRoute : System.Web.UI.Page
{
    Dictionary<TextBox, UploadedImage> descriptionMap = new Dictionary<TextBox, UploadedImage>();
    Route route;
    List<MyRadioButton> buttons = new List<MyRadioButton>();
    string mainImage = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!LoginState.TestLogin())
            return;

        if (string.IsNullOrEmpty(RouteName.Value))
            RouteName.Value = Request.Params["Route"];

        string script = string.Format(@"
function getRouteName(){{
    return document.getElementById('{0}').value; 
}}
function getGpsField(){{
    return document.getElementById('{1}'); 
}}
function getUpdateImagesButton(){{
    return document.getElementById('{2}'); 
}}", RouteName.ClientID, TextBoxGPS.ClientID, ReloadImages.ClientID);

        ScriptManager.RegisterClientScriptBlock(
            this,
            GetType(),
            "ScriptFunctions",
            script,
            true);


        MapFrame.Attributes["src"] = "map.aspx?EditMode=true&Route=" + RouteName.Value;
        MapFrame.Attributes["onload"] = "frameLoaded(this);";
        UploadImageFrame.Attributes["src"] = "UploadFile.aspx?Route=" + RouteName.Value;
        UploadImageFrame.Attributes["onload"] = "imagesUploaded(this);";
        route = DBHelper.GetRoute(RouteName.Value);

        if (!IsPostBack && route != null)
        {
            TextBoxTitle.Text = route.Title;
            TextBoxDescription.Text = route.Description;
            TextBoxCiclyng.Text = route.Cycling.ToString();
            TextBoxDifficulty.Text = route.Difficulty;
            DifficultyFromString();
            mainImage = route.Image;
        }

        BuildImageControls();
    }

    private void BuildImageControls()
    {
        string tableId = "TableImages";
        Control c = UpdatePanelImages.ContentTemplateContainer.FindControl(tableId);
        if (c != null)
            UpdatePanelImages.ContentTemplateContainer.Controls.Remove(c);
        UploadedImages list = UploadedImages.FromSession(RouteName.Value);
        Table table = new Table();
        table.ID = tableId;
        table.Style[HtmlTextWriterStyle.Position] = "relative";
        table.Style[HtmlTextWriterStyle.MarginLeft] = "auto";
        table.Style[HtmlTextWriterStyle.MarginRight] = "auto";
        table.Style[HtmlTextWriterStyle.TextAlign] = "center";
        UpdatePanelImages.ContentTemplateContainer.Controls.Add(table);
        TableRow row = null;

        int col = 0;
        for (int i = 0; i < list.Count; i++)
        {
            UploadedImage ui = list.GetAt(i);
            if (ui.IsDeleted)
                continue;
            if (col == 0)
            {
                row = new TableRow();
                table.Rows.Add(row);
            }

            TableCell cell = new TableCell();
            row.Cells.Add(cell);
            cell.CssClass = "ImageCell";
            cell.Width = Unit.Percentage(33.33333);

            Panel container = new Panel();
            container.Style[HtmlTextWriterStyle.Display] = "inline-block";

            cell.Controls.Add(container);

            string fileName = Path.GetFileName(ui.File);
            if (string.IsNullOrEmpty(mainImage))
                mainImage = fileName;

            MyImageButton ib = new MyImageButton(ui);
            ib.ID = "IB_" + fileName;
            ib.ImageUrl = "~/Images/Recycle.png";
            ib.ToolTip = "Elimina immagine";
            ib.Click += new ImageClickEventHandler(ib_Click);
            ib.CssClass = "DeleteImage";
            ib.CausesValidation = false;
            ib.Attributes["onmouseout"] = "normalDeleteImage(this);";
            ib.Attributes["onmouseover"] = "hoverDeleteImage(this);";

            container.Controls.Add(ib);

            UpdatePanel panel = new UpdatePanel();
            panel.ChildrenAsTriggers = true;
            panel.UpdateMode = UpdatePanelUpdateMode.Conditional;
            container.Controls.Add(panel);


            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = string.Format("~/RouteImage.axd?Route={0}&Image={1}", RouteName.Value, HttpUtility.UrlEncode(ui.File));
            img.Style[HtmlTextWriterStyle.PaddingLeft] = img.Style[HtmlTextWriterStyle.PaddingRight] = "20px";
            img.Style[HtmlTextWriterStyle.PaddingTop] = "20px";
            panel.ContentTemplateContainer.Controls.Add(img);

            TextBox tb = new TextBox();
            tb.Style[HtmlTextWriterStyle.Display] = "block";
            tb.Width = Unit.Pixel(200);
            tb.ID = "I_" + Path.GetFileName(ui.File);
            tb.Text = ui.Description;
            tb.CausesValidation = true;
            descriptionMap[tb] = ui;
            tb.TextChanged += new EventHandler(tb_TextChanged);
            tb.AutoPostBack = true;
            tb.Style[HtmlTextWriterStyle.MarginLeft] = tb.Style[HtmlTextWriterStyle.MarginRight] = "auto";

            panel.ContentTemplateContainer.Controls.Add(tb);

            RequiredFieldValidator val = new RequiredFieldValidator();
            val.ID = "V_" + tb.ID;
            val.ControlToValidate = tb.ID;
            val.ErrorMessage = "Descrizione immagine obbligatoria!";
            val.SetFocusOnError = true;
            val.Display = ValidatorDisplay.Dynamic;
            panel.ContentTemplateContainer.Controls.Add(val);


            MyRadioButton rb = new MyRadioButton(ui);
            buttons.Add(rb);
            rb.Style[HtmlTextWriterStyle.Display] = "block";
            rb.Width = Unit.Pixel(200);
            rb.ID = "CB_" + fileName;
            rb.Text = "Immagine principale";
            rb.Style[HtmlTextWriterStyle.MarginLeft] = rb.Style[HtmlTextWriterStyle.MarginRight] = "auto";
            rb.Checked = fileName == mainImage;
            rb.CausesValidation = false;
            rb.EnableViewState = true;
            container.Controls.Add(rb);

            if (++col == 3)
                col = 0;
        }

    }

    void ib_Click(object sender, ImageClickEventArgs e)
    {
        MyImageButton ib = ((MyImageButton)sender);
        ib.Image.IsDeleted = true;
        MyRadioButton first = null;
        for (int i = 0; i < buttons.Count; i++)
        {
            //la prima immagine non cancellata diventa la default
            MyRadioButton btn = buttons[i];
            if (btn.Image.IsDeleted)
            {
                btn.Checked = false;
            }
            else if (first == null)
                first = btn;
        }
        if (first != null)
        {
            first.Checked = true;
            mainImage = Path.GetFileName(first.Image.File);
        }
        BuildImageControls();
        UpdatePanelImages.Update();
    }

    void tb_TextChanged(object sender, EventArgs e)
    {
        TextBox tb = ((TextBox)sender);
        UploadedImage ui = descriptionMap[tb];
        ui.Description = tb.Text;
    }

    protected void ButtonDelete_Click(object sender, EventArgs e)
    {
        try
        {
            string routeName = RouteName.Value;
            if (string.IsNullOrEmpty(routeName))
                return;
            //non posso eliminare una traccia che appartiene ad un alro utente
            //(se mai riesco a editarla)
            if (route != null && route.OwnerId != LoginState.User.Id)
                return;

            string path = PathFunctions.GetRoutePathFromName(routeName);
            TryDeleting(path);
            path = PathFunctions.GetWorkingPath(path);
            TryDeleting(path);
            if (route != null)
                DBHelper.DeleteRoute(route);
            GpxParser.RemoveFromSession(routeName);
            UploadedImages.RemoveFromSession(routeName);
            //forza il ricaricamento della pagina
            Response.Redirect(Request.Url.ToString(), true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ErrorDeleting", string.Format("alert('Errore durante l'eliminazione: {0}.');", ex.Message), true);
        }
    }

    private static void TryDeleting(string path)
    {
        //faccio 10 tentativi
        int count = 0;
        while (true)
        {
            try
            {
                if (Directory.Exists(path))
                    Directory.Delete(path, true);
                break;
            }
            catch
            {
                count++;
                if (count > 10)
                    throw;
            }
        }
    }

    protected void ButtonSave_Click(object sender, EventArgs e)
    {
        try
        {
            string routeName = RouteName.Value;
            if (string.IsNullOrEmpty(routeName))
                return;
            //non posso salvare una traccia che appartiene ad un alro utente
            //(se mai riesco a editarla)
            if (route != null && route.OwnerId != LoginState.User.Id)
                return;

            //calcolo l'immagine principale
            UploadedImages list = UploadedImages.FromSession(RouteName.Value);
            mainImage = "";
            foreach (MyRadioButton btn in buttons)
                if (btn.Checked)
                {
                    mainImage = Path.GetFileName(btn.Image.File);
                    break;
                }

            if (route == null)
                route = new Route();
           
            //assegno i dati al record
            route.Image = mainImage;
            route.Name = routeName;
            route.OwnerId = LoginState.User.Id;
            int c = 0;
            if (int.TryParse(TextBoxCiclyng.Text, out c))
                route.Cycling = c;
            route.Title = TextBoxTitle.Text;
            route.Description = TextBoxDescription.Text;
            route.Difficulty = TextBoxDifficulty.Text;

            //salvo il file gpx
            GpxParser parser = GpxParser.FromSession(routeName);
            if (parser != null)
            {
                string gpxFile = PathFunctions.GetGpxPathFromRouteName(routeName);
                string path = Path.GetDirectoryName(gpxFile);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                parser.Save(gpxFile);
            }

            //salvo le immagini
            string imageFolder = PathFunctions.GetImagePathFromRouteName(routeName);
            if (!Directory.Exists(imageFolder))
                Directory.CreateDirectory(imageFolder);
            foreach (UploadedImage ui in list)
                ui.SaveTo(imageFolder);
            //elimino una eventuale cache
            Helper.ClearImageCache(imageFolder);
            //forzo la generazione dei thumbnails
            Helper.GetImageCache(imageFolder);
            //salvo il record
            DBHelper.SaveRoute(route);

            if (CheckBoxSendMail.Checked)
            {
                //mando una mail agli utenti registrati
                string msg = string.Format("Ciao biker!<br/>L'utente {0} ha inserito o modificato il percorso<br/><a target=\"route\" href=\"{1}\">{2}</a><br/>Scarica il tracciato e vieni a provarlo!<br/><br/>MTB Scout",
                    LoginState.User.DisplayName,
                    "http://www.mtbscout.it" + route.GetRouteUrl(false),
                    route.Title
                    );
                foreach (MTBUser u in DBHelper.Users)
                    if (u.SendMail)
                        Helper.SendMail(u.EMail, null, null, "Inserimento/modifica percorso", msg, true);
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "MessageOK", "alert('Informazioni salvate correttamente.');", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Error", string.Format("alert('Errore durante il salvataggio: {0}.');", ex.Message), true);
        }

    }

    protected void DropDownListDown_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateDownExplanation();
        DifficultyToString();
    }

    private void UpdateDownExplanation()
    {
        if (DropDownListDown.SelectedIndex >= 1)
        {
            LabelDown.Text = Helper.DifficultyMap[DropDownListDown.SelectedValue];
            LabelDown.BackColor = Helper.DifficultyMapColor[DropDownListDown.SelectedValue];
            LabelDown.ForeColor = Color.Black;
        }
    }


    protected void DropDownListClimb_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateClimbExplanation();
        DifficultyToString();
    }

    private void UpdateClimbExplanation()
    {
        if (DropDownListClimb.SelectedIndex >= 1)
        {
            LabelClimb.Text = Helper.DifficultyMap[DropDownListClimb.SelectedValue];
            LabelClimb.BackColor = Helper.DifficultyMapColor[DropDownListClimb.SelectedValue];
            LabelClimb.ForeColor = Color.Black;
        }
    }


    protected void CheckBoxClimb_CheckedChanged(object sender, EventArgs e)
    {
        DifficultyToString();
    }
    protected void CheckBoxDown_CheckedChanged(object sender, EventArgs e)
    {
        DifficultyToString();
    }

    private void DifficultyToString()
    {
        if (DropDownListDown.SelectedIndex < 1 || DropDownListClimb.SelectedIndex < 1)
        {
            TextBoxDifficulty.Text = "";
            return;
        }

        TextBoxDifficulty.Text =
             DropDownListClimb.SelectedValue +
             (CheckBoxClimb.Checked ? "+" : "") +
             '/' +
             DropDownListDown.SelectedValue +
             (CheckBoxDown.Checked ? "+" : "");
    }
    private void DifficultyFromString()
    {
        string diff = TextBoxDifficulty.Text;
        string pattern = "(?<Diff>(TC)|(MC)|(BC)|(OC)|(EC))(?<Slope>\\+?)";
        MatchCollection mm = Regex.Matches(diff, pattern);
        if (mm.Count != 2)
            return;
        Match up = mm[0];
        UpdateListValue(DropDownListClimb, up.Groups["Diff"].Value);
        CheckBoxClimb.Checked = up.Groups["Slope"].Value == "+";
        Match down = mm[1];
        UpdateListValue(DropDownListDown, down.Groups["Diff"].Value);
        CheckBoxDown.Checked = down.Groups["Slope"].Value == "+";
        UpdateClimbExplanation();
        UpdateDownExplanation();
    }

    private void UpdateListValue(DropDownList ddl, string val)
    {

        for (int i = 1; i < ddl.Items.Count; i++)
            if (ddl.Items[i].Value == val)
            {
                ddl.SelectedIndex = i;
                break;
            }
    }

}
class MyRadioButton : RadioButton
{
    UploadedImage image;
    public MyRadioButton(UploadedImage image)
    {
        this.AutoPostBack = false;
        this.GroupName = "MainImage";
        this.image = image;
    }
    public UploadedImage Image { get { return image; } }
}

class MyImageButton : ImageButton
{
    UploadedImage image;
    public MyImageButton(UploadedImage image)
    {
        this.image = image;
    }
    public UploadedImage Image { get { return image; } }
}