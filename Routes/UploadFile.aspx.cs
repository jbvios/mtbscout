using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using MTBScout;

public partial class Routes_UploadFile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		string routeName = Request.QueryString["Route"];
		for (int i = 0; i < Request.Files.Count; i++)
		{
			HttpPostedFile file = Request.Files[i];

			UploadedImage ui = new UploadedImage();
			ui.FileName = file.FileName;
			ui.Image = (Bitmap)Bitmap.FromStream(file.InputStream);
			ui.Description = "";

			ui.AddToSession(routeName);
		}
    }
}
