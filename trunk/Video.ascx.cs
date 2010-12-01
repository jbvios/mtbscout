using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MTBScout;

public partial class Video : System.Web.UI.UserControl
{
	public string VideoUrl { get; set; }
	public string PreviewUrl { get; set; }
	public string Title { get; set; }
	public string VideoWidth { get; set; }
	public string VideoHeight { get; set; }
	
	public Video()
	{
		VideoWidth = "372";
		VideoHeight = "300";
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		
	}
	protected void Page_Load (object sender, EventArgs e)
	{

	}

	public string Value
	{
		get
		{
            return string.Format("value=\"file={0}&image={1}\"", 
                PathFunctions.GetUrlFromPath(Page.MapPath(VideoUrl), false),
                PathFunctions.GetUrlFromPath(Page.MapPath(PreviewUrl), false));
		}
	}
}
