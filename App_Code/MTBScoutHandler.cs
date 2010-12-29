using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;

namespace MTBScout
{
    public class MTBScoutHandler : IHttpHandler, IReadOnlySessionState
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
			string file = context.Request.QueryString["File"];
			if (!string.IsNullOrEmpty(file))
			{
				context.Response.ContentType = "application/octet-stream";
				context.Response.TransmitFile(file);
				return;
			}
            string routeName = context.Request.QueryString["Route"];
            string imageName = context.Request.QueryString["Image"];

            UploadedImage img = UploadedImage.FromSession(routeName, imageName);
			string folder = PathFunctions.GetImagePathFromRouteName(routeName);
            if (img != null)
				img.SaveTo(context.Response);
        }
    }
}