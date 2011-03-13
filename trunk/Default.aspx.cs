using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using MTBScout.Entities;
using MTBScout;
using System.Drawing;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
		string title, url;
		if (string.IsNullOrEmpty(RandomImage2.ImageUrl))
		{
			RandomImage1.ImageUrl = GetRandomImageUrl(out title, out url);
			RandomImage1.ToolTip = title;
			A1.NavigateUrl = url;
		}
		else
		{
			RandomImage1.ImageUrl = RandomImage2.ImageUrl;
			RandomImage1.ToolTip = RandomImage2.ToolTip;
			A1.NavigateUrl = A2.NavigateUrl;
		}

		if (string.IsNullOrEmpty(RandomImage3.ImageUrl))
		{
			RandomImage2.ImageUrl = GetRandomImageUrl(out title, out url);
			RandomImage2.ToolTip = title;
			A2.NavigateUrl = url;
		}
		else
		{
			RandomImage2.ImageUrl = RandomImage3.ImageUrl;
			RandomImage2.ToolTip = RandomImage3.ToolTip;
			A2.NavigateUrl = A3.NavigateUrl;
		}
		RandomImage3.ImageUrl = GetRandomImageUrl(out title, out url);
		RandomImage3.ToolTip = title;
		A3.NavigateUrl = url;

		ScriptManager.RegisterStartupScript(
			this,
			GetType(),
			"StartScrolling",
			"setTimeout(function() { moveRouteImage(document.getElementById('ImageLayer'), 0); }, 3000);",
			true);
		
    }

	private string GetRandomImageUrl (out string title, out string pageUrl)
	{
		Random r = new Random(DateTime.Now.Second);
		while (true)
		{
			int routeIdx = r.Next(DBHelper.Routes.Count());
			Route route = DBHelper.Routes.ElementAt(routeIdx);
			ImageCache cache = Helper.GetImageCache(PathFunctions.GetImagePathFromRouteName(route.Name));
			if (cache.reducedUrls.Length == 0)
				continue;
			int imageIdx = r.Next(cache.fileUrls.Length);
			Size sz = cache.sizes[imageIdx];
			if (sz.Height > sz.Width) 
				continue;
			title = route.Title;
			pageUrl = route.GetRouteUrl(false);
			return cache.reducedUrls[imageIdx];
		}
	}
}
