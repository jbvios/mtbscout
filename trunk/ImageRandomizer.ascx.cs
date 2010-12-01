using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Threading;

public partial class ImageRandomizer : System.Web.UI.UserControl
{
	private string imageFolder;
	public string ImageFolder
	{ get { return imageFolder; } set { imageFolder = value; } }

	public string AlternateText 
	{ get { return RandomImage.AlternateText; } set { RandomImage.AlternateText = value; } }
    protected void Page_Load(object sender, EventArgs e)
    {
		string key = "ImageFile" + this.ID;
		object o = Page.Session[key];
		if (o == null)
		{
			string[] files = Directory.GetFiles(MapPathSecure(ImageFolder), "*.jpg");
			Random r = new Random(DateTime.Now.Millisecond);
			int index = r.Next(files.Length);
			o = Path.Combine(ImageFolder, Path.GetFileName(files[index]));
			Page.Session[key] = o;
		}

		RandomImage.ImageUrl = (string)o;
    }
}
