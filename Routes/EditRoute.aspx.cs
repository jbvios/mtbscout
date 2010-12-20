using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MTBScout;
using System.IO;
using MTBScout.Entities;

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
}}", RouteName.ClientID, TextBoxGPS.ClientID);
            
        ScriptManager.RegisterClientScriptBlock(
            this,
            GetType(),
            "ScriptFunctions",
            script,
            true);
        

        MapFrame.Attributes["src"] = "map.aspx?EditMode=true&Route=" + RouteName.Value;
        MapFrame.Attributes["onload"] = "frameLoaded(this);";
        if (IsPostBack)
            return;
        route = DBHelper.GetRoute(RouteName.Value);
        if (route != null)
        {
            TextBoxTitle.Text = route.Title;
            TextBoxDescription.Text = route.Description;
        }

    }
    protected void ButtonSave_Click(object sender, EventArgs e)
    {
        string routeName = RouteName.Value;
        if (string.IsNullOrEmpty(routeName))
            return;

        string path = PathFunctions.GetRoutePathFromName(routeName);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        GpxParser parser = GpxParser.FromSession(routeName);
        if (parser != null)
            parser.Save(Path.Combine(path, "track.gpx"));
    }
}
