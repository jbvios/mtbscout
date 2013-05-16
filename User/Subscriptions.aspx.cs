using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MTBScout.Entities;
using System.Globalization;

public partial class User_Subscriptions : System.Web.UI.Page
{
    EventSubscriptor[] subscriptors;
    public const int TorrigliaId = 2;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);


    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Check.Text.Length > 0)
            throw new ApplicationException("FuckYou!");
        //if (!LoginState.TestLogin())
        //	return;
        if (!Page.IsPostBack)
        {
            //CommandField bf = new CommandField();
            //bf.DeleteImageUrl = "~/Images/recycle.png";
            //bf.HeaderText = "Cancella";
            //bf.ButtonType = ButtonType.Image;
            //bf.ShowDeleteButton = true;
            //GridViewSubscriptions.Columns.Add(bf);

            //bf = new CommandField();
            //bf.EditImageUrl = "~/Images/edit.png";
            //bf.HeaderText = "Modifica";
            //bf.ButtonType = ButtonType.Image;
            //bf.ShowEditButton = true;
            //GridViewSubscriptions.Columns.Add(bf);

            //BoundField dcf = new BoundField();
            //dcf.HeaderText = "Nome";
            //dcf.DataField = "Name";
            //GridViewSubscriptions.Columns.Add(dcf);

            //dcf = new BoundField();
            //dcf.HeaderText = "Cognome";
            //dcf.DataField = "Surname";
            //GridViewSubscriptions.Columns.Add(dcf);

            //dcf = new BoundField();
            //dcf.HeaderText = "Mail";
            //dcf.DataField = "EMail";
            //GridViewSubscriptions.Columns.Add(dcf);

            //dcf = new BoundField();
            //dcf.HeaderText = "Data di nascita";
            //dcf.DataField = "BirthDate";
            //GridViewSubscriptions.Columns.Add(dcf);

            //dcf = new BoundField();
            //dcf.HeaderText = "Sesso";
            //dcf.DataField = "GenderDescription";
            //GridViewSubscriptions.Columns.Add(dcf);
        }
        //LoadSubscriptors();
    }

    //private void LoadSubscriptors()
    //{
    //    subscriptors = DBHelper.GetSubscriptors(LoginState.User.Id, SuperEnduroId);

    //    GridViewSubscriptions.DataSource = subscriptors;
    //    GridViewSubscriptions.DataBind();
    //}

    protected void CustomValidatorBirth_ServerValidate(object source, ServerValidateEventArgs args)
    {
        DateTime dummy;
        args.IsValid = ParseDate(args.Value, out dummy);
    }

    private static bool ParseDate(string dateString, out DateTime date)
    {
        return DateTime.TryParse(dateString, CultureInfo.CurrentCulture, DateTimeStyles.None, out date);
    }
    protected void ButtonSave_Click(object sender, EventArgs e)
    {
        DateTime dt;
        if (!ParseDate(TextBoxBirthDate.Text, out dt))
            return;
        EventSubscriptor sbscr = DBHelper.LoadSubscriptor(TextBoxMail.Text);
        if (sbscr == null)
        {
            sbscr = new EventSubscriptor();
            sbscr.EMail = TextBoxMail.Text;
            int dummy;
            int.TryParse(SubscriptionId.Value, out dummy);
            sbscr.Id = dummy;
        }
        sbscr.BirthDate = dt;
        sbscr.EventId = TorrigliaId;
        sbscr.UserId = 0;// LoginState.User.Id;
        sbscr.Name = TextBoxName.Text;
        sbscr.Surname = TextBoxSurname.Text;

        sbscr.GenderNumber = (short)RadioButtonListGender.SelectedIndex;
        DBHelper.SaveSubscriptor(sbscr);
        Helper.SendMail(sbscr.EMail, null, "info@mtbscout.it", "Conferma preiscrizione Tourist Trophy Torriglia", "Ti confermiamo l'avvenuta iscrizione, grazie per esserti registrato al Tourist Trophy Torriglia. Buon divertimento!", false);
                
        //LoadSubscriptors();

        RefreshCurrentSubscriptor();

        Page.ClientScript.RegisterStartupScript(GetType(), "MessageOK", "alert('Informazioni salvate correttamente. Grazie per esserti registrato.');", true);
        ViewState.Clear();
    }

    private void RefreshCurrentSubscriptor()
    {
        SubscriptionId.Value = "";
        TextBoxBirthDate.Text = "";
        SubscriptionId.Value = "";
        TextBoxName.Text = "";
        TextBoxSurname.Text = "";
        TextBoxMail.Text = "";
        RadioButtonListGender.SelectedIndex = 0;
    }
    protected void GridViewSubscriptions_RowEditing(object sender, GridViewEditEventArgs e)
    {
        if (e.NewEditIndex == -1 || e.NewEditIndex > subscriptors.Length - 1)
            return;
        EventSubscriptor sbscr = subscriptors[e.NewEditIndex];

        TextBoxBirthDate.Text = sbscr.BirthDate.ToString();
        SubscriptionId.Value = sbscr.Id.ToString();
        TextBoxName.Text = sbscr.Name;
        TextBoxSurname.Text = sbscr.Surname;
        TextBoxMail.Text = sbscr.EMail;
        RadioButtonListGender.SelectedIndex = sbscr.GenderNumber;
    }
    protected void GridViewSubscriptions_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (e.RowIndex == -1 || e.RowIndex > subscriptors.Length - 1)
            return;
        EventSubscriptor sbscr = subscriptors[e.RowIndex];
        DBHelper.DeleteSubscriptor(sbscr);

        //LoadSubscriptors();
        RefreshCurrentSubscriptor();
    }
}
