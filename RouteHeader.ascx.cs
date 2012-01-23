using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.IO;
using System.Xml;
using MTBScout;
using System.Globalization;
using MTBScout.Entities;

public partial class RouteHeader : System.Web.UI.UserControl
{

	public string RouteName
	{
		get;
		set;
	}

	public bool HideTitle { get; set; }

	public void SetFontWidth(int pixels)
	{
		Table.Style[HtmlTextWriterStyle.FontSize] = pixels + "px";
	}
	protected void Page_Load(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(RouteName))
			RouteName = Path.GetFileName(Page.MapPath("."));
		Route r = DBHelper.GetRoute(RouteName);
        if (r == null)
        {
            return;
        }
		GpxParser parser = r.Parser;
		string routeLenght = Math.Round(parser.Distance3D / 1000, 1).ToString(CultureInfo.InvariantCulture);
		string routeTotalHeight = Convert.ToInt32(parser.TotalClimb).ToString();
		string routeMaxHeight = Convert.ToInt32(parser.MaxElevation).ToString();
		string routeMinHeight = Convert.ToInt32(parser.MinElevation).ToString();


		Page.Header.Title = r.Title;
		if (HideTitle)
			Title.Visible = false;
		else
			Title.InnerText = r.Title;
		MTBUser user = DBHelper.LoadUser(r.OwnerId);
		if (user != null)
			Owner.InnerText = user.DisplayName;
		Lenght.InnerText = routeLenght + " Km";
		//TotalHeight.InnerText = routeTotalHeight + " m";

		MaxHeight.InnerText = routeMaxHeight + " m";
		MinHeight.InnerText = routeMinHeight + " m";
		Cycle.InnerText = r.Cycling.ToString() + "%";
		Difficulty.InnerText = r.Difficulty;
        Difficulty.Attributes["title"] = Helper.GetDifficultyExplanation(r.Difficulty);
		int votes = 0;
		double w = DBHelper.GetMediumRank(r, out votes);
		RankIndicator.Style.Add(HtmlTextWriterStyle.Width, Convert.ToInt16(w * 10) + "px");
		RankIndicator.Style.Add(HtmlTextWriterStyle.Height, "20px");
		RankIndicator.Style.Add(HtmlTextWriterStyle.BackgroundColor, "blue");
		RankIndicator.Style.Add(HtmlTextWriterStyle.Display, "inline");
		RankIndicator.Style.Add(HtmlTextWriterStyle.Position, "absolute");

		RankLabel.InnerText = string.Format("Valutazione ({0} voti):", votes);
		RankDetailLink.NavigateUrl = "~/Routes/RouteRankDetail.aspx?RouteName=" + r.Name;
		RankDetailLink.Target = "RouteDetail" + r.Id;
	}
}
