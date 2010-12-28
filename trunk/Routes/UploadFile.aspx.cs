using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using MTBScout;
using System.IO;

public partial class Routes_UploadFile : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		string routeName = Request.QueryString["Route"];
		for (int i = 0; i < Request.Files.Count; i++)
		{
			try
			{
				HttpPostedFile file = Request.Files[i];

				//creo un nuovo file in session
				UploadedImage ui = new UploadedImage(Guid.NewGuid().ToString());
				ui.Image = (Bitmap)Bitmap.FromStream(file.InputStream);
				ui.Description = Helper.GetImageTitle(ui.Image);
				if (string.IsNullOrEmpty(ui.Description))
					ui.Description = Path.GetFileNameWithoutExtension(file.FileName);
				ui.AddToSession(routeName);
			}
			catch 
			{
				//file non valido, mancato upload!
			}
		}
	}
}
