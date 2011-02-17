﻿using System;
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
	List<MyButton> buttons = new List<MyButton>();

	protected void Page_Load(object sender, EventArgs e)
	{
        if (!LoginState.TestLogin())
        {
            FormsAuthentication.RedirectToLoginPage();
            return;
        }
       
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

		string mainImage = null;
		UploadedImages list = UploadedImage.FromSession(RouteName.Value);
		if (!IsPostBack && route != null)
		{
			TextBoxTitle.Text = route.Title;
			TextBoxDescription.Text = route.Description;
			TextBoxCiclyng.Text = route.Cycling.ToString();
			TextBoxDifficulty.Text = route.Difficulty;
			DifficultyFromString();
			mainImage = route.Image;
        }
		 
		Table table = new Table();
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
			if (col == 0)
			{
				row = new TableRow();
				table.Rows.Add(row);
			}

			TableCell cell = new TableCell();
			row.Cells.Add(cell);
			cell.CssClass = "ImageCell";
			cell.Width = Unit.Percentage(33.33333);

			UpdatePanel panel = new UpdatePanel();
			panel.ChildrenAsTriggers = true;
			panel.UpdateMode = UpdatePanelUpdateMode.Conditional;
			cell.Controls.Add(panel);

			System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
			img.ImageUrl = string.Format("~/RouteImage.axd?Route={0}&Image={1}", RouteName.Value, HttpUtility.UrlEncode(ui.File));
			img.Style[HtmlTextWriterStyle.PaddingLeft] = img.Style[HtmlTextWriterStyle.PaddingRight] = "20px";
			img.Style[HtmlTextWriterStyle.PaddingTop] = "20px";
			panel.ContentTemplateContainer.Controls.Add(img);

			TextBox tb = new TextBox();
			tb.Style[HtmlTextWriterStyle.Display] ="block";
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
			panel.ContentTemplateContainer.Controls.Add(val);


			MyButton cb = new MyButton(panel);
			buttons.Add(cb);
			cb.Style[HtmlTextWriterStyle.Display] = "block";
			cb.Width = Unit.Pixel(200);
			string fileName = Path.GetFileName(ui.File);
			cb.ID = "CB_" + fileName;
			cb.Text = "Immagine principale";

			cb.Image = ui;
			if (mainImage == null)
				mainImage = fileName;

			cb.Checked = fileName == mainImage;
			cb.CausesValidation = false;
			cb.EnableViewState = true;
			cell.Controls.Add(cb);

			if (++col == 3)
				col = 0;
		}
	}

	void tb_TextChanged(object sender, EventArgs e)
	{
		TextBox tb = ((TextBox)sender);
		UploadedImage ui = descriptionMap[tb];
		ui.Description = tb.Text;
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

			UploadedImages list = UploadedImage.FromSession(RouteName.Value);
			if (list.Count == 0)
			{
				ScriptManager.RegisterStartupScript(this, GetType(), "NoImages", "alert('Occorre caricare almeno una immagine.');", true);
				return;
			}
			string mainImage = null;
			foreach (MyButton btn in buttons)
				if (btn.Checked)
				{
					mainImage = Path.GetFileName(btn.Image.File);
					break;
				}
			if (string.IsNullOrEmpty(mainImage))
			{
				ScriptManager.RegisterStartupScript(this, GetType(), "NoImages", "alert('Occorre indicare un'immagine principale.');", true);
				return;
			}

			string imageFolder = PathFunctions.GetImagePathFromRouteName(RouteName.Value);
			foreach (UploadedImage ui in list)
				ui.SaveTo(imageFolder);

			GpxParser parser = GpxParser.FromSession(routeName);
			if (parser != null)
			{
				string gpxFile = PathFunctions.GetGpxPathFromRouteName(routeName);
				string path = Path.GetDirectoryName(gpxFile);
				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);

				parser.Save(gpxFile);
			}

			if (route == null)
				route = new Route();

			route.Image = mainImage;
			route.Name = routeName;
			route.OwnerId = LoginState.User.Id;
			int c = 0;
			if (int.TryParse(TextBoxCiclyng.Text, out c))
				route.Cycling = c;
			route.Title = TextBoxTitle.Text;
			route.Description = TextBoxDescription.Text;
			route.Difficulty = TextBoxDifficulty.Text;

			DBHelper.SaveRoute(route);
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
class MyButton : RadioButton 
{
	public MyButton(UpdatePanel panel)
	{
		Panel = panel;
		this.AutoPostBack = false;
		this.GroupName = "MainImage";
	}
	public UpdatePanel Panel { get; set; }
	public UploadedImage Image { get; set; }
}
