using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MTBScout;
using MTBScout.Entities;

public partial class Routes_Route : System.Web.UI.Page
{
    public string[] DescriptionParagraphs;
    protected override void OnInit(EventArgs e)
    { 
        base.OnInit(e);
        string routeName = Request.Params["Route"];
        Route r = DBHelper.GetRoute(routeName);
        if (r == null)
        {
            RouteHeader1.Visible = false;
            DownloadGpsTrack1.Visible = false;
            ImageIterator1.Visible = false;
            UnexistingRoute.Visible = true;
        }
        else
        {
            RouteHeader1.RouteName = routeName;
            DownloadGpsTrack1.RouteName = routeName;
            ImageIterator1.ImagesPath = PathFunctions.GetImagePathFromRouteName(routeName);
        }
        if (r == null || string.IsNullOrEmpty(r.Description))
        {
            DescriptionParagraphs = new string[0];
            return;
        }
        DescriptionParagraphs = r.Description.Split(new char[] { '\n' });
       
    }
    
}
