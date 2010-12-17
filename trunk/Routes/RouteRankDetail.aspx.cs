using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MTBScout.Entities;

public partial class Routes_RouteRankDetail : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			string routeName = Request.Params["RouteName"];
			
			Route r = DBHelper.GetRoute(routeName);
			if (r != null)
			{
				List<RankData> values = new List<RankData>();
				RouteTitle.InnerText = r.Title;

				IList<Rank> ranks = DBHelper.GetRanks(r.Id);
				foreach (Rank rk in ranks)
				{
					MTBUser user = DBHelper.LoadUser(rk.UserId);
					RankData rd = new RankData();
					rd.Name = user.Name;
					rd.Surname = user.Surname;
					rd.Nickname = user.Nickname;
					rd.Rank = rk.RankNumber;
					values.Add(rd);
				}
				Repeater1.DataSource = values;
				Repeater1.DataBind();
			}
		}
	}
}

internal struct RankData
{
	public string Name { get; set; }
	public string Surname { get; set; }
	public string Nickname { get; set; }
	public int Rank { get; set; }
}