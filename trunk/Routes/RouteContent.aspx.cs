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

    public void GenerateMarkers()
    {

        Response.Write("<script type=\"text/javascript\">\r\n");
        Response.Write("function addMarkers(){\r\n");

        foreach (string routeFolder in Directory.GetDirectories(MapPath(".")))
        {
            string routeFile = Path.Combine(routeFolder, "route.xml");

            if (!File.Exists(routeFile))
                continue;
            XmlDocument routeDoc = new XmlDocument();
            routeDoc.Load(routeFile);

            string caption = routeDoc.DocumentElement.GetAttribute("title");
            string url = PathFunctions.GetUrlFromPath(MapPath(routeDoc.DocumentElement.GetAttribute("url")), false).Replace("'", "\\'"); 
            string thumbUrl = routeDoc.DocumentElement.GetAttribute("imageUrl");
            string gpxFile = Path.Combine(routeFolder, "track.gpx");
            GpxParser parser = Helper.GetGpxParser(gpxFile);
            if (parser == null)
                continue;

           
            //TrackPoint p = parser.Tracks[0].Segments[0].Points[0];
            GenericPoint p = parser.MediumPoint;
            string color = "blue";
            string name = caption.Replace("'", "\\'");
            string description = string.Format("<iframe scrolling=\"no\" frameborder=\"no\" src=\"/RouteData.aspx?name={0}\"/>", Path.GetFileName(routeFolder));
            string icon = "";
            string imageFolder = PathFunctions.GetImagePathFromGpx(gpxFile);
            string imageFile = Path.Combine(imageFolder, Path.GetFileName(thumbUrl));
            if (!File.Exists(imageFile))
            {
                string[] files = Directory.GetFiles(imageFolder, "*.jpg");
                if (files.Length > 0)
                    imageFile = files[0];
                else
                    imageFile = "";
            }
            string thumbFile = imageFile.Length == 0 ? "" : PathFunctions.GetThumbFile(imageFile);
            string photo = PathFunctions.GetUrlFromPath(thumbFile, false).Replace("'", "\\'"); 
            Response.Write(string.Format(
            "GV_Draw_Marker({{ lat: {0}, lon: {1}, name: '{2}', desc: '{3}', color: '{4}', icon: '{5}', photo: '{6}', url: '{7}' }});\r\n",
                p.lat.ToString(System.Globalization.CultureInfo.InvariantCulture),
                p.lon.ToString(System.Globalization.CultureInfo.InvariantCulture),
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
       

        Response.Write("}\r\n");
        Response.Write("</script>\r\n");
    }


    public void GenerateLegendItems()
    {
        
    }


    protected void Page_Load(object sender, EventArgs e)
    {
       
    }




}
