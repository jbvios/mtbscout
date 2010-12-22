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
using NHibernate;
using MTBScout.Entities;

public partial class Map : System.Web.UI.Page
{

    public void GenerateMarkers()
    {
        Response.Write("<script type=\"text/javascript\">\r\n");
        Response.Write("function addMarkers(){\r\n");
		bool editMode = Request.QueryString["EditMode"] == "true";
        foreach (Route r in GetRoutes())
        {
            string routeFolderPath = PathFunctions.GetRoutePathFromName(r.Name);
            string url = 
				editMode
				? PathFunctions.GetUrlFromPath(PathFunctions.EditRoutePage, false).Replace("'", "\\'") + "?Route=" + r.Name
				: (string.IsNullOrEmpty(r.Page)
					? PathFunctions.GetUrlFromPath(PathFunctions.RoutesPage, false).Replace("'", "\\'") + "?Route=" + r.Name
					: PathFunctions.GetUrlFromPath(Path.Combine(routeFolderPath, r.Page), false).Replace("'", "\\'"));

            string gpxFile = Path.Combine(routeFolderPath, "track.gpx");

            GpxParser parser = Helper.GetGpxParser(gpxFile);
            if (parser == null)
                continue;

            //TrackPoint p = parser.Tracks[0].Segments[0].Points[0];
            GenericPoint p = parser.MediumPoint;
            string color = "blue";
            string name = r.Title.Replace("'", "\\'");
            string description = string.Format("<iframe scrolling=\"no\" frameborder=\"no\" src=\"/RouteData.aspx?name={0}\"/>", r.Name);
            string icon = "";
            string imageFolder = PathFunctions.GetImagePathFromGpx(gpxFile);
            string imageFile = Path.Combine(imageFolder, r.Image);
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

    private IEnumerable<Route> GetRoutes()
    {
        int ownerId;
        string filter = Request.QueryString["UserId"];
        
        return (string.IsNullOrEmpty(filter) || !int.TryParse(filter, out ownerId))
            ? DBHelper.Routes 
            : DBHelper.GetRoutes(ownerId);
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
