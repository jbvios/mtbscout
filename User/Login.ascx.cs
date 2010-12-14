using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MTBScout.Entities;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.RelyingParty;
using System.Web.Security;

public partial class User_Login : System.Web.UI.UserControl
{
    protected void OpenIdLogin_LoggedIn(object sender, OpenIdEventArgs e)
    {
        //recupero l'utente da db, oppure lo creo al volo se non esiste
        MTBUser user = DBHelper.LoadUser(e.ClaimedIdentifier.ToString());

        //non esiste? allora lo creo e lo metto nella sessione come NewUser, quindi 
        //rimando alla pagina User che, solo dopo aver inserito i dati obblicatori, lo metterà
        //finalmente in sessione come User
        if (user == null)
        {
            user = new MTBUser();
            user.OpenId = e.ClaimedIdentifier;
            user.Surname = "";
            ClaimsResponse profileFields = e.Response.GetExtension<ClaimsResponse>();
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
            FormsAuthentication.RedirectToLoginPage();//devo completare i dati utente
        }
        else
        {
            LoginState.User = user;
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
