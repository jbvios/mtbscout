using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Globalization;
using System.Xml;
using MTBScout;
using System.Web.Caching;
using System.IO;
using MTBScout.Entities;

public partial class Map : System.Web.UI.Page
{

    GpxParser parser;
    public void GenerateCustomOptions(bool editMode)
    {
        Response.Write("<script type=\"text/javascript\">\r\n");
        Response.Write("function addCustomOptions(){\r\n");
        if (editMode)
        {
            Response.Write(@"
            gv_options.width = 750;  
            gv_options.height = 350;  
            gv_options.mousewheel_zoom = false;
            gv_options.full_screen = false;");

        }
        Response.Write("}\r\n");
        Response.Write("</script>\r\n");
    }
    public void GenerateMarkers()
    {

        Response.Write("<script type=\"text/javascript\">\r\n");
        Response.Write("function addMarkers(){\r\n");

        if (parser != null && !string.IsNullOrEmpty(parser.SourceFile))
        {
            string thumbsFolder = PathFunctions.GetThumbsFolder(parser.SourceFile);

            foreach (WayPoint wp in parser.WayPoints)
            {
                bool hasPhoto =
                    !string.IsNullOrEmpty(wp.link) &&
                    wp.link.EndsWith(".jpg", StringComparison.InvariantCultureIgnoreCase);

                string color = hasPhoto
                    ? "blue"
                    : "red";

                string name = wp.name.Replace("'", "\\'");
                string description = wp.description.Replace("'", "\\'");
                string icon = hasPhoto
                    ? "/Images/camera_icon.png"
                    : "";
                string photo = hasPhoto
                    ? PathFunctions.GetUrlFromPath(Path.Combine(thumbsFolder, Path.GetFileName(wp.link)), false).Replace("'", "\\'")
                    : "";
                string url = hasPhoto
                    ? PathFunctions.GetUrlFromPath(wp.link, false).Replace("'", "\\'")
                    : "";
                Response.Write(string.Format(
                "GV_Draw_Marker({{ lat: {0}, lon: {1}, name: '{2}', desc: '{3}', color: '{4}', icon: '{5}', photo: '{6}', url: '{7}' }});\r\n",
                    wp.lat.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    wp.lon.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    name,
                    description,
                    color,
                    icon,
                    photo,
                    url));
            }
        }
        Response.Write("}\r\n");
        Response.Write("</script>\r\n");
    }
    /// <summary>
    /// Genera una linea tipo:
    /// track.push({ color:'#E60000', points:[ [44.518971,9.054657], [44.518977,9.054656] ] });
    /// che rappresenta un segmento di traccia
    /// </summary>
    public void GenerateTrack()
    {
        Helper.GenerateTrackCode(parser, Response, true);
   
    }


    public void GenerateLegendItems()
    {
        if (parser == null)
            return;

        int nItems = 4;

        double step = (parser.MaxElevation - parser.MinElevation) / nItems;
        for (int i = 0; i <= nItems; i++)
        {
            double ele = parser.MinElevation + step * i;
            Response.Write(" <div class=\"gv_legend_item\" style=\"text-align:right; padding-right:5px\">\r\n");
            Response.Write(string.Format("<span style=\"color: {0}\">{1}</span>\r\n</div>\r\n",
                ColorProvider.GetColorString(ele, parser.MinElevation, parser.MaxElevation), Convert.ToInt32(ele)));
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string routeName = Request.QueryString["Route"];

            if (!IsPostBack)
            {
                Route r = DBHelper.GetRoute(routeName);
                if (r != null)
                {
                    string gpxFile = PathFunctions.GetGpxPathFromRouteName(routeName);
                   
                    parser = Helper.GetGpxParser(gpxFile);
                    if (parser != null)
                        parser.LoadPhothos();
                    Title = r.Title;
                }
            }
            bool editMode = Request.QueryString["EditMode"] == "true";
            GenerateCustomOptions(editMode);
            ChooseRoute.Visible = editMode;
            if (editMode && FileUploadGpx.HasFile)
            {
                try
                {
                    GpxParser p = new GpxParser();
                    p.Parse(FileUploadGpx.FileContent);
                    if (p.Tracks.Count == 0)
                        return;

                    parser = p;
                }
                catch
                {
                }

            }

            if (parser == null)
                parser = GpxParser.FromSession(routeName);
            else
                parser.ToSession(routeName);
        }
        finally
        {
           // MapContainer.Visible = parser != null;
        }
    }

    

}
