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
using NHibernate;
using System.Linq.Expressions;
using NHibernate.Criterion;
using System.Web.Security;

public partial class Login : System.Web.UI.Page
{
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        string fbId = Request.Params["fbId"];
        if (!string.IsNullOrEmpty(fbId))
            LogUser(fbId, null);
    }

    protected void OpenIdLogin_LoggedIn(object sender, OpenIdEventArgs e)
    {
        LogUser(e.ClaimedIdentifier.ToString(), e.Response.GetExtension<ClaimsResponse>());

    }

    private void LogUser(string openId, ClaimsResponse profileFields)
    {
        //recupero l'utente da db, oppure lo creo al volo se non esiste
        MTBUser user = DBHelper.LoadUser(openId);

        //non esiste? allora lo creo e lo metto nella sessione come NewUser, quindi 
        //rimando alla pagina User che, solo dopo aver inserito i dati obblicatori, lo metterà
        //finalmente in sessione come User
        if (user == null)
        {
            user = new MTBUser();
            user.OpenId = openId;
            user.Surname = "";
           
            if (profileFields != null)
            {
                user.Name = profileFields.FullName;
                user.EMail = profileFields.Email;
                user.Nickname = profileFields.Nickname;
                if (profileFields.Gender != null)
                {
                    switch (profileFields.Gender.Value)
                    {
                        case Gender.Male:
                            user.Gender = MTBUser.GenderType.Male;
                            break;
                        case Gender.Female:
                            user.Gender = MTBUser.GenderType.Female;
                            break;
                        default:
                            break;
                    }
                }

                if (profileFields.BirthDate != null)
                    user.BirthDate = profileFields.BirthDate.Value;
            }

            LoginState.NewUser = user;
            Response.Redirect("User.aspx");
        }
        else
        {
            LoginState.User = user;
            FormsAuthentication.RedirectFromLoginPage("", false);
        }
    }

    protected void OpenIdLogin_LoggingIn(object sender, OpenIdEventArgs e)
    {
        e.Request.AddExtension(new ClaimsRequest
        {
            BirthDate = DemandLevel.Require,
            Country = DemandLevel.Require,
            Email = DemandLevel.Require,
            FullName = DemandLevel.Require,
            Gender = DemandLevel.Require,
            PostalCode = DemandLevel.Require,
            TimeZone = DemandLevel.Require,
            Language = DemandLevel.Require,
            Nickname = DemandLevel.Require
        });
    }
}


