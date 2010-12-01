using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Blog : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		string url = Request.Params["redirect"];
		if (string.IsNullOrEmpty(url))
			url = "http://mtbscout.blogspot.com";
		ClientScript.RegisterStartupScript(GetType(), "initFrame", string.Format("document.getElementById('blogFrame').src = '{0}';", url), true);
    }
}
