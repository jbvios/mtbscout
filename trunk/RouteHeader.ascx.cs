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
        Route r = DBHelper.GetRoute(RouteName);

        GpxParser parser = r.Parser;
        string routeLenght = Math.Round(parser.Distance3D / 1000, 1).ToString(CultureInfo.InvariantCulture);
        string routeTotalHeight = Convert.ToInt32(parser.TotalClimb).ToString();
        string routeMaxHeight = Convert.ToInt32(parser.MaxElevation).ToString();
        string routeMinHeight = Convert.ToInt32(parser.MinElevation).ToString();


        Page.Header.Title = r.Title;
        if (HideTitle)
            Title.Visible = false;
        else
            Title.InnerText = r.Title;

        Lenght.InnerText = routeLenght + " Km";
        TotalHeight.InnerText = routeTotalHeight + " m";
        MaxHeight.InnerText = routeMaxHeight + " m";
        MinHeight.InnerText = routeMinHeight + " m";
        Cycle.InnerText = r.Cycling.ToString() + "%";
        Difficulty.InnerText = r.Difficulty;
    }
}
