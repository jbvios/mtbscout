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
		List<UploadedImage> list = UploadedImage.FromSession(routeName);

		string temporaryFolder = PathFunctions.GetTempPath();
		
		for (int i = 0; i < Request.Files.Count; i++)
		{
			try
			{
				HttpPostedFile file = Request.Files[i];

				//creo un nuovo file in session
				string filePath = Path.Combine(temporaryFolder, 
					string.Format("{0}-{1}.jpg", (list.Count + 1).ToString("000"), routeName));
				UploadedImage ui = new UploadedImage(filePath, file.InputStream);
				list.Add(ui);
			}
			catch
			{
				//file non valido, mancato upload!
			}
		}
	}
}
