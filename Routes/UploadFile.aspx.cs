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
                List<UploadedImage> list = UploadedImage.FromSession(routeName);
                UploadedImage ui = new UploadedImage(list.Count.ToString("000") + ".jpg");
                ui.Image = (Bitmap)Bitmap.FromStream(file.InputStream);
                ui.Description = Helper.GetImageTitle(ui.Image);
                list.Add(ui);
            }
            catch
            {
                //file non valido, mancato upload!
            }
        }
    }
}
