using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.IO;
using System.Xml;
using MTBScout;
using System.Globalization;
using MTBScout.Entities;

public partial class RouteHeader : System.Web.UI.UserControl
{
    private string routeTitle = "";

    public string RouteTitle
    {
        get { return routeTitle; }
        set { routeTitle = value; }
    }
    private string routeLenght = "";

    public string RouteLenght
    {
        get { return routeLenght; }
        set { routeLenght = value; }
    }
    private string routeTotalHeight = "";

    public string RouteTotalHeight
    {
        get { return routeTotalHeight; }
        set { routeTotalHeight = value; }
    }
    private string routeMaxSlope = "";

    public string RouteMaxSlope
    {
        get { return routeMaxSlope; }
        set { routeMaxSlope = value; }
    }
    private string routeMaxHeight = "";

    public string RouteMaxHeight
    {
        get { return routeMaxHeight; }
        set { routeMaxHeight = value; }
    }
    private string routeMinHeight = "";

    public string RouteMinHeight
    {
        get { return routeMinHeight; }
        set { routeMinHeight = value; }
    }
    private string routeCycle = "";

    public string RouteCycle
    {
        get { return routeCycle; }
        set { routeCycle = value; }
    }
    private string routeDifficulty = "";

    public string RouteDifficulty
    {
        get { return routeDifficulty; }
        set { routeDifficulty = value; }
    }

    public string RouteName
    {
        get;
        set;
    }

    public bool HideTitle { get; set; }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(RouteName))
            RouteName = Path.GetFileName(Page.MapPath("."));
        if (!Page.IsPostBack)
        {
            string routeFolder = PathFunctions.GetRoutePathFromName(RouteName);

            string gpxFile = Path.Combine(routeFolder, "track.gpx");

            GpxParser parser = Helper.GetGpxParser(gpxFile);
            RouteLenght = Math.Round(parser.Distance3D / 1000, 1).ToString(CultureInfo.InvariantCulture);
            RouteTotalHeight = Convert.ToInt32(parser.TotalClimb).ToString();
            RouteMaxHeight = Convert.ToInt32(parser.MaxElevation).ToString();
            RouteMinHeight = Convert.ToInt32(parser.MinElevation).ToString();
            Route r = DBHelper.GetRoute(RouteName);
            RouteTitle = r.Title;
            RouteCycle = r.Cycling.ToString();
            RouteDifficulty = r.Difficulty;
        }

        Page.Header.Title = RouteTitle;
        if (HideTitle)
            Title.Visible = false;
        else
            Title.InnerText = RouteTitle;
        
        Lenght.InnerText = RouteLenght + " Km";
        TotalHeight.InnerText = RouteTotalHeight + " m";
        MaxHeight.InnerText = RouteMaxHeight + " m";
        MinHeight.InnerText = RouteMinHeight + " m";
        Cycle.InnerText = RouteCycle + "%";
        Difficulty.InnerText = RouteDifficulty;
    }
}
