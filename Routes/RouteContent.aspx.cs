using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Globalization;
using System.Xml;
using System.Linq;
using MTBScout;
using System.Web.Caching;
using System.IO;
using NHibernate;
using MTBScout.Entities;

public partial class Map : System.Web.UI.Page
{
	bool editMode = false;
	
	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		editMode = Request.QueryString["EditMode"] == "true";
	}
	public void GenerateCustomOptions()
	{
		Response.Write("<script type=\"text/javascript\">\r\n");
		Response.Write("function addCustomOptions(){\r\n");
		if (editMode && Routes.Count() == 0)
		{
			Response.Write(@"
			gv_options.zoom = 5;");

		}
		Response.Write("}\r\n");
		Response.Write("</script>\r\n");
	}

    public void GenerateMarkers()
    {
        Response.Write("<script type=\"text/javascript\">\r\n");
        Response.Write("function addMarkers(){\r\n");
		foreach (Route r in Routes)
        {
			string url = r.GetRouteUrl(editMode);

			string gpxFile = PathFunctions.GetGpxPathFromRouteName(r.Name);

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
            string imageFile = Path.Combine(imageFolder, string.IsNullOrEmpty (r.Image) ? "" : r.Image);
            if (!File.Exists(imageFile))
            {
                string[] files = Directory.GetFiles(imageFolder, "*.jpg");
                if (files.Length > 0)
                    imageFile = files[0];
                else
                    imageFile = "";
            }
            string thumbFile = imageFile.Length == 0 ? "" : PathFunctions.GetThumbFile(imageFile);
            string photo = thumbFile.Length == 0 ? "" : PathFunctions.GetUrlFromPath(thumbFile, false).Replace("'", "\\'");
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

	

	IEnumerable<Route> routes = null;
    private IEnumerable<Route> Routes
    {
		get
		{
			if (routes == null)
			{
				int ownerId;
				string filter = Request.QueryString["UserId"];

				routes =(string.IsNullOrEmpty(filter) || !int.TryParse(filter, out ownerId))
					? DBHelper.Routes
					: DBHelper.GetRoutes(ownerId);
			}
			return routes;
		}
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
		GenerateCustomOptions();
    }




}
