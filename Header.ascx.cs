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
        //SpotLeft.Visible = ShowAds;
        //SpotRight.Visible = ShowAds;

    }
    

    
}
