﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using MTBScout;

public partial class Test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string folder = Server.MapPath("~/Routes");
        Response.Write(PathFunctions.RootPath);
        Delete(folder, "profile.png");
        Delete(folder, "map.html");
		Delete(Server.MapPath("~/public/workingdata"), "track.gz");
        Delete(Server.MapPath("~"), "thumbs.db");

    }

    private void Delete(string folder, string filter)
    {
        foreach (string file in Directory.GetFiles(folder, filter, SearchOption.AllDirectories))
        {
            File.Delete(file);
            Response.Write("<p>" + file + "</p>");
        }
    }

    private void CopyFiles(string folder)
    {
        foreach (String sub in Directory.GetDirectories(folder))
        {
            if (string.Compare(Path.GetFileName(sub), "thumbs", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                Directory.Delete(sub, true);
                //string parent = Path.GetDirectoryName(sub);
                //foreach (string file in Directory.GetFiles(sub, "*.jpg"))
                //{
                //    string target = Path.Combine(parent, Path.GetFileName(file));
                //    File.Copy(file, target, true);
                //    Response.Write(target + Environment.NewLine);
                //}
                Response.Write("<p>" + sub + "</p>");
            }
            else
            {
                CopyFiles(sub);
            }
        }
    }
}

