using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MTBScout.Entities;
using System.Globalization;

public partial class SubscriptionList : System.Web.UI.Page
{
    EventSubscriptor[] subscriptors;
    public const int TorrigliaId = 2;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);


    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BoundField dcf = new BoundField();
            dcf.HeaderText = "Nome";
            dcf.DataField = "Name";
            GridViewSubscriptions.Columns.Add(dcf);

            dcf = new BoundField();
            dcf.HeaderText = "Cognome";
            dcf.DataField = "Surname";
            GridViewSubscriptions.Columns.Add(dcf);

            dcf = new BoundField();
            dcf.HeaderText = "Gruppo";
            dcf.DataField = "Club";
            GridViewSubscriptions.Columns.Add(dcf);

            dcf = new BoundField();
            dcf.HeaderText = "Mail";
            dcf.DataField = "EMail";
            GridViewSubscriptions.Columns.Add(dcf);

            dcf = new BoundField();
            dcf.HeaderText = "Data di nascita";
            dcf.DataField = "BirthDateFormatted";
            GridViewSubscriptions.Columns.Add(dcf);

            dcf = new BoundField();
            dcf.HeaderText = "Sesso";
            dcf.DataField = "GenderDescription";
            GridViewSubscriptions.Columns.Add(dcf);
        }
        LoadSubscriptors();
    }

    private void LoadSubscriptors()
    {
        subscriptors = DBHelper.GetSubscriptors(TorrigliaId);

        GridViewSubscriptions.DataSource = subscriptors;
        GridViewSubscriptions.DataBind();
    }
}
