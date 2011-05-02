using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

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
}
