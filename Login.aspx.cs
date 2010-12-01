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

public partial class Login : System.Web.UI.Page
{
	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
	}
    protected void Page_Load(object sender, EventArgs e)
    {
		if (Page.IsPostBack)
		{
		}
    }

	
	protected void OpenIdLogin1_LoggingIn(object sender, OpenIdEventArgs e)
	{
		this.prepareRequest(e.Request);
	}

	private void prepareRequest(IAuthenticationRequest request)
	{
		// Collect the PAPE policies requested by the user.
		List<string> policies = new List<string>();
		//foreach (ListItem item in this.papePolicies.Items)
		//{
		//    if (item.Selected)
		//    {
		//        policies.Add(item.Value);
		//    }
		//}

		// Add the PAPE extension if any policy was requested.
		if (policies.Count > 0)
		{
			var pape = new PolicyRequest();
			foreach (string policy in policies)
			{
				pape.PreferredPolicies.Add(policy);
			}

			request.AddExtension(pape);
		}
	}

	/// <summary>
	/// Fired upon login.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="DotNetOpenAuth.OpenId.RelyingParty.OpenIdEventArgs"/> instance containing the event data.</param>
	/// <remarks>
	/// Note, that straight after login, forms auth will redirect the user
	/// to their original page. So this page may never be rendererd.
	/// </remarks>
	protected void OpenIdLogin1_LoggedIn(object sender, OpenIdEventArgs e)
	{
		
	}

	/// <summary>
	/// Strong-typed bag of session state.
	/// </summary>
	public class State
	{
		public static ClaimsResponse ProfileFields
		{
			get { return HttpContext.Current.Session["ProfileFields"] as ClaimsResponse; }
			set { HttpContext.Current.Session["ProfileFields"] = value; }
		}

		public static FetchResponse FetchResponse
		{
			get { return HttpContext.Current.Session["FetchResponse"] as FetchResponse; }
			set { HttpContext.Current.Session["FetchResponse"] = value; }
		}

		public static string FriendlyLoginName
		{
			get { return HttpContext.Current.Session["FriendlyUsername"] as string; }
			set { HttpContext.Current.Session["FriendlyUsername"] = value; }
		}

		public static PolicyResponse PapePolicies
		{
			get { return HttpContext.Current.Session["PapePolicies"] as PolicyResponse; }
			set { HttpContext.Current.Session["PapePolicies"] = value; }
		}

		public static string GoogleAccessToken
		{
			get { return HttpContext.Current.Session["GoogleAccessToken"] as string; }
			set { HttpContext.Current.Session["GoogleAccessToken"] = value; }
		}

		public static void Clear()
		{
			ProfileFields = null;
			FetchResponse = null;
			FriendlyLoginName = null;
			PapePolicies = null;
			GoogleAccessToken = null;
		}
	}
	protected void btnLoginToGoogle_Click(object sender, EventArgs e)
	{

	}
	protected void OpenIdLogin1_LoggedIn1(object sender, OpenIdEventArgs e)
	{
		State.FriendlyLoginName = e.Response.FriendlyIdentifierForDisplay;
		State.ProfileFields = e.Response.GetExtension<ClaimsResponse>();
		State.PapePolicies = e.Response.GetExtension<PolicyResponse>();
		ClaimsResponse fetch = e.Response.GetExtension(typeof(ClaimsResponse)) as ClaimsResponse;  
          
	}
}
