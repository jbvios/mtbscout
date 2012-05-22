using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class school_School : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        FBLike.Attributes["src"] = string.Format(
             "http://www.facebook.com/widgets/like.php?href={0}",
             HttpUtility.UrlEncode(Page.Request.Url.ToString()));

    }
}
