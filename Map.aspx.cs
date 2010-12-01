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

public partial class Map : System.Web.UI.Page
{

    GpxParser parser;
    string gpxFile;

    public void GenerateMarkers()
    {

        Response.Write("<script type=\"text/javascript\">\r\n");
        Response.Write("function addMarkers(){\r\n");

        int prog = 0;
        string thumbsFolder = PathFunctions.GetThumbsFolder(gpxFile);
            
        foreach (WayPoint wp in parser.WayPoints)
        {
            bool hasPhoto =  
                !string.IsNullOrEmpty(wp.link) &&
                wp.link.EndsWith(".jpg", StringComparison.InvariantCultureIgnoreCase);
            
            string color = hasPhoto 
                ? "blue" 
                : "red";
            string name = hasPhoto 
                ? Helper.GetImageCaption(prog++, wp.link).Replace("'", "\\'")
                : wp.name.Replace("'", "\\'");
            string description = wp.description.Replace("'", "\\'");
            string icon = hasPhoto 
                ? "camera" 
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
        Response.Write("<script type=\"text/javascript\">\r\n");
        Response.Write("function addTracks(){\r\n");
        int trackIndex = 0;
        foreach (Track trk in parser.Tracks)
        {
            Response.Write(string.Format(@"
                t = {0}; 
                track = [];
                trk_info[t] = track;
                track['name'] = '{1}';
                track['desc'] = '{2}'; 
                track['clickable'] = true;
                track['width'] = 3; 
                track['opacity'] = 0.9;
                track['outline_color'] = '#000000';
                track['outline_width'] = 0;
                track['fill_color'] = '#E60000'; 
                track['fill_opacity'] = 0;
                trkSeg = [];
                trk_segments[t] = trkSeg;",
               ++trackIndex,
               trk.Name,
               trk.Description));

            foreach (TrackSegment seg in trk.Segments)
            {
                TrackPoint[] reducedPoints = seg.GetReducedPoints();
                for (int i = 0; i < reducedPoints.Length - 1; i++)
                {
                    TrackPoint p1 = reducedPoints[i];
                    TrackPoint p2 = reducedPoints[i + 1];

                    string color = ColorProvider.GetColorString(p1.ele, parser.MinElevation, parser.MaxElevation);
                    //devo usare InvariantCulture per avere il punto come separatore dei decimali
                    Response.Write(string.Format(
                        "trkSeg.push({{ color:'{0}', points:[ [{1},{2}], [{3},{4}] ] }});\r\n",
                        color,
                        p1.lat.ToString(System.Globalization.CultureInfo.InvariantCulture),
                        p1.lon.ToString(System.Globalization.CultureInfo.InvariantCulture),
                        p2.lat.ToString(System.Globalization.CultureInfo.InvariantCulture),
                        p2.lon.ToString(System.Globalization.CultureInfo.InvariantCulture)
                        ));

                }
            }

            Response.Write("GV_Draw_Track(t);");
        }

        Response.Write("}\r\n");
        Response.Write("</script>\r\n");
    }


    public void GenerateLegendItems()
    {
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
        gpxFile = PathFunctions.RootPath + Request.QueryString["GpxUrl"];
        parser = Helper.GetGpxParser(gpxFile);
        parser.LoadPhothos();
        Title = Request.QueryString["MapTitle"];
    }




}
