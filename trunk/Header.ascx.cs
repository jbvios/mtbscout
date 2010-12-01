using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SiteHeader : System.Web.UI.UserControl
{
	private bool showAds = true;

	public bool ShowAds
	{
		get { return showAds; }
		set { showAds = value; }
	}

    protected void Page_Load(object sender, EventArgs e)
    {
		LiteralControl c = new LiteralControl("<META NAME=\"Description\" CONTENT=\"mountain bike MTB montoggio scout bicicletta bike biker bikers pentemina scrivia\">");
		Page.Header.Controls.Add(c);

		SpotLeft.Visible = ShowAds;
		SpotRight.Visible = ShowAds;
    }

	
}
