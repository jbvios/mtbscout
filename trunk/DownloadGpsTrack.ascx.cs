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

        if (string.IsNullOrEmpty(RouteName))
            RouteName = Path.GetFileName(Page.MapPath("."));
        string gpxFullPath = Path.Combine(PathFunctions.GetRoutePathFromName(RouteName), "track.gpx");
        GpxParser parser = Helper.GetGpxParser(gpxFullPath);
        if (parser == null)
            return;
       
        MapLink.NavigateUrl = string.Format("~/Routes/Map.aspx?Route={0}", RouteName);
        ProfileImage.Src = Helper.GenerateProfileFile(gpxFullPath);

        int countryCode = 0;
        countryCode = parser.CountryCode;
        string zipPath = parser.ZippedFile;
        HyperLinkToGps.NavigateUrl = PathFunctions.GetUrlFromPath(zipPath, true);

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
            Rank r = DBHelper.GetRank(user.Id, GetRoute().Id);
            Rank.SelectedIndex = r == null ? -1 : r.RankNumber - 1;
        }
    }


    protected void Rank_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!LoginState.TestLogin())
            return;
        try
        {
            byte vote = Convert.ToByte(Rank.SelectedIndex + 1);
            DBHelper.SaveRank(LoginState.User.Id, GetRoute().Id, vote);
            RankMessage.Text = String.Format("Il tuo voto ({0}) è stato registrato, grazie per il contributo.", vote);
        }
        catch (Exception ex)
        {
            RankMessage.Text = String.Format("Si è verificato un errore: {0}", ex.Message);
        }
        RankMessage.Visible = true;
    }

    private Route GetRoute()
    {
        if (string.IsNullOrEmpty(RouteName))
            RouteName = Path.GetFileName(Page.MapPath("."));
        Route r = DBHelper.GetRoute(RouteName);
        return r;
    }

}
