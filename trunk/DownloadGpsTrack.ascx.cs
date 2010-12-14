using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MTBScout;
using MTBScout.Entities;
using System.IO;

public partial class DownloadGpsTrack : System.Web.UI.UserControl
{
    public string RouteName
    {
        get;
        set;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;

        string gpxFullPath = Page.MapPath("track.gpx");
        string gpxRelPath = PathFunctions.GetRelativePath(gpxFullPath);
        gpxRelPath = gpxRelPath.Replace('\\', '/');
        if (!gpxRelPath.StartsWith("/"))
            gpxRelPath = "/" + gpxRelPath;
        MapLink.NavigateUrl = string.Format("~/Map.aspx?GpxUrl={0}&MapTitle={1}", gpxRelPath, Page.Header.Title);

        ProfileImage.Src = Helper.GenerateProfileFile(gpxRelPath);

        int countryCode = 0;
        GpxParser parser = Helper.GetGpxParser(gpxFullPath);
        if (parser != null)
        {
            countryCode = parser.CountryCode;
            string zipPath = parser.ZippedFile;
            HyperLinkToGps.NavigateUrl = PathFunctions.GetUrlFromPath(zipPath, true);

        }
        if (countryCode != 0)
            MeteoFrame.Attributes["src"] = string.Format("http://www.ilmeteo.it/script/meteo.php?id=free&citta={0}", countryCode);
        else
            MeteoFrame.Visible = false;

        FBLike.Attributes["src"] = string.Format(
            "http://www.facebook.com/widgets/like.php?href={0}",
            HttpUtility.UrlEncode(Page.Request.Url.ToString()));
        
        MTBUser user = LoginState.User;
        if (user == null)
            Rank.SelectedIndex = -1;
        else
        {
            Rank r = DBHelper.GetRank(user, GetRoute());
            Rank.SelectedIndex = r == null ? -1 : r.RankNumber - 1;
        }
    }


    protected void Rank_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!LoginState.TestLogin())
            return;
        DBHelper.SaveRank(LoginState.User, GetRoute(), (Convert.ToByte(Rank.SelectedIndex + 1)));
    }

    private Route GetRoute()
    {
        if (string.IsNullOrEmpty(RouteName))
            RouteName = Path.GetFileName(Page.MapPath("."));
        Route r = DBHelper.GetRoute(RouteName);
        return r;
    }

}
