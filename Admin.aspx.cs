using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using MTBScout.Entities;
using MTBScout;
using System.IO;

public partial class Admin : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!LoginState.IsAdmin())
			FormsAuthentication.RedirectToLoginPage();
	}
	protected void ButtonExecQuery_Click(object sender, EventArgs e)
	{
		TextBoxResult.Text = "";
		try
		{
			GridViewSelectResults.DataSource = (Object)DBHelper.ExecQuery(TextBoxQuery.Text);
			GridViewSelectResults.DataBind();
			TextBoxResult.Text = "Query eseguita con successo";
		}
		catch (Exception ex)
		{
			WriteException(ex);
		}
	}

	private void WriteException(Exception ex)
	{
		TextBoxResult.Text += ex.Message;
		if (ex.InnerException != null)
			WriteException(ex.InnerException);
	}
	protected void ButtonOffsetDate_Click(object sender, EventArgs e)
	{
		string original = PathFunctions.GetImagePathFromRouteName(TextBoxRouteName.Text);
		if (!Directory.Exists(original))
			return;
		TimeSpan offset;
		if (!TimeSpan.TryParse(TextBoxOffset.Text, out offset))
			return;
		
		foreach (string file in Directory.GetFiles(original, "*.jpg"))
		{
			DateTime photoTime;
			double latidudeRef, latitude, longitudeRef, longitude;
			Helper.GetImageInfos(file, out photoTime, out latidudeRef, out latitude, out longitudeRef, out longitude);
			photoTime += offset;
			Helper.SetCreationTime(file, photoTime);

		}
		
	}
}

