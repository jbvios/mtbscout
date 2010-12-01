using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class Download : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		string file = Server.MapPath(Request["file"]);

		if (File.Exists(file))
		{
			Response.ClearContent();
			switch (Path.GetExtension(file).ToLower())
			{
				case ".pdf":
					Response.ContentType = "application/pdf";
					break;
				default:
					Response.ContentType = "text/xml";
					break;
			}

			Response.TransmitFile(file);
		}
		else
			Response.Write(string.Format("File non trovato: {0}", file));
    }
}
