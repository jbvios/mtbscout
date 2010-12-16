using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RouteData : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        RouteHeader1.RouteName = Request.Params["name"];
		RouteHeader1.SetFontWidth(11);
    }
}
