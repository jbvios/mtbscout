using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Drawing.Imaging;
using System.Drawing;

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
            string routeName = context.Request.QueryString["Route"];
            string imageName = context.Request.QueryString["Image"];

            UploadedImage img = UploadedImage.FromSession(routeName, imageName);
            if (img != null)
            {
                using (Bitmap bmp = Helper.CreateThumbnail(img.Image, 200))
                {
                    context.Response.ContentType = "image/jpeg";
                    bmp.Save(context.Response.OutputStream, ImageFormat.Jpeg);
                }
            }
        }
    }
}