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

public partial class Routes_EditRoute : System.Web.UI.Page
{
	Route route;
	protected void Page_Load(object sender, EventArgs e)
	{
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

		if (!IsPostBack)
		{
			route = DBHelper.GetRoute(RouteName.Value);
			if (route != null)
			{
				TextBoxTitle.Text = route.Title;
				TextBoxDescription.Text = route.Description;
				TextBoxCiclyng.Text = route.Cycling.ToString();
				TextBoxDifficulty.Text = route.Difficulty;
				DifficultyFromString();
			}


		}
		Table table = new Table();
		table.Style[HtmlTextWriterStyle.Position] = "relative";
		table.Style[HtmlTextWriterStyle.MarginLeft] = "auto";
		table.Style[HtmlTextWriterStyle.MarginRight] = "auto";
		table.Style[HtmlTextWriterStyle.TextAlign] = "center";
		UpdatePanelImages.ContentTemplateContainer.Controls.Add(table);
		TableRow row = null;

		List<UploadedImage> list = UploadedImage.FromSession(RouteName.Value);
		int col = 0;
		foreach (UploadedImage ui in list)
		{
			if (col == 0)
			{
				row = new TableRow();
				table.Rows.Add(row);
			}

			TableCell cell = new TableCell();
			row.Cells.Add(cell);
			cell.CssClass = "ImageCell";
			cell.Width = Unit.Percentage(33.33333);
			System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
			img.ImageUrl = string.Format("~/RouteImage.axd?Route={0}&Image={1}", RouteName.Value, ui.Id);
			img.Style[HtmlTextWriterStyle.PaddingLeft] = img.Style[HtmlTextWriterStyle.PaddingRight] = "20px";
			cell.Controls.Add(img);

			TextBox tb = new TextBox();
			tb.Style[HtmlTextWriterStyle.Display] ="block";
			tb.Width = Unit.Pixel(200);
			tb.ID = "I_" + ui.Id;
			tb.Text = ui.Description;
			tb.CausesValidation = true;
			
			tb.Style[HtmlTextWriterStyle.MarginLeft] = tb.Style[HtmlTextWriterStyle.MarginRight] = "AUTO";
			
			cell.Controls.Add(tb);

			RequiredFieldValidator val = new RequiredFieldValidator();
			val.ControlToValidate = tb.ID;
			val.ErrorMessage = "Campo obbligatorio!";
			val.SetFocusOnError = true;
			cell.Controls.Add(val);


			if (++col == 3)
				col = 0;
		}


	}


	protected void ButtonSave_Click(object sender, EventArgs e)
	{
		string routeName = RouteName.Value;
		if (string.IsNullOrEmpty(routeName))
			return;

		GpxParser parser = GpxParser.FromSession(routeName);
		if (parser != null)
		{
			string gpxFile = PathFunctions.GetGpxPathFromRouteName(routeName);
			string path = Path.GetDirectoryName(gpxFile);
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			parser.Save(gpxFile);
		}
		bool added = false;
		added = (route == null);

		route = new Route();
		route.Name = routeName;
		route.OwnerId = LoginState.User.Id;
		int c = 0;
		if (int.TryParse(TextBoxCiclyng.Text, out c))
			route.Cycling = c;
		route.Title = TextBoxTitle.Text;
		route.Description = TextBoxDescription.Text;
		route.Difficulty = TextBoxDifficulty.Text;
		DBHelper.SaveRoute(route);
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
