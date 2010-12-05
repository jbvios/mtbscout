using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using DotNetOpenAuth.OpenId.Extensions.ProviderAuthenticationPolicy;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.RelyingParty;
using DotNetOpenAuth.OpenId;
using MTBScout.Entities;

public partial class Login : System.Web.UI.Page
{
	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
	}
    protected void Page_Load(object sender, EventArgs e)
    {
		
    }

	protected void OpenIdLogin_LoggedIn(object sender, OpenIdEventArgs e)
	{
        LoginState.ClaimedIdentifier = e.ClaimedIdentifier;
		LoginState.FriendlyLoginName = e.Response.FriendlyIdentifierForDisplay;
        LoginState.ProfileFields = e.Response.GetExtension<ClaimsResponse>();
	}
}


