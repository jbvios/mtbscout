using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MTBScout;

public partial class Events : System.Web.UI.Page
{
	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
        PDFXC.HRef = PathFunctions.GetUrlFromPath(Page.MapPath("XCGiu2007.pdf"), true); 
		PDFXC.Target = "_blank";
	}

}
