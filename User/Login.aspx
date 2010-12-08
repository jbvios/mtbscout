<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Login.aspx.cs" Inherits="Login" %>

<%@ Register Assembly="DotNetOpenAuth" Namespace="DotNetOpenAuth.OpenId.RelyingParty"
    TagPrefix="rp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Accesso registrato</title>
    <style type="text/css">
        .Text
        {
            width: 350px;
        }
        .box
        {
            border: medium solid #FF0000;
            position: relative;
            top: 30px;
            padding: 20px;
            margin-bottom: 20px;
        }
        .LoginBox
        {
            padding: 20px;
        }
        .LoginBox table
        {
            margin-left: auto;
            margin-right: auto;
            position: relative;
        }
        .style1
        {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPanel" runat="Server">
    <div id="ContentPanel" class="ContentPanel">
        <div class="box">
            <h1>
                Inserisci le tue credenziali</h1>
            <%--<div style="padding: 20px;" title = "Usa le tue credenziali Facebook">
			
				<fb:login-button></fb:login-button></div>--%>
            <p>
                Inserisci le tue credenziali OpenId per accedere all&#39;area riservata. Pensi di
                non avere un OpenId? Forse non � cos�: molti portali in cui sei probabilmente gi�
                registrato offrono questo servizio; di seguito ti offriamo alcuni esempi, <a href="http://www.openid.it/dove/"
                    target="_blank">qui</a> trovi ulteriori informazioni.</p>
            <p>
                Ma cos� OpenId? E&#39; un servizio che ti permette di usare lo stesso nome e password
                che usi nel tuo portale preferito anche per effettuare l&#39;accesso ad altri siti;
                in questo modo si evita il proliferare di password con tutti i problemi di sicurezza
                connessi alla gestione delle stesse. </p>
            <p class="style1">
               <strong> MTBScout NON gestisce o conserva informazioni
                relative alle password.</strong></p>
            <div style="text-align: left; padding-left: 30px; padding-right: 30px;">
                <table style="width:400px; text-align:center; margin-left:auto;margin-right:auto;">
                    <tr>
                        <td>
                            <div>
                                <rp:OpenIdButton runat="server" ImageUrl="~/images/google.png" Text="Accedi con Google"
                                    ID="googleLoginButton" Identifier="https://www.google.com/accounts/o8/id" 
                                    OnLoggedIn="OpenIdLogin_LoggedIn" OnLoggingIn="OpenIdLogin_LoggingIn"/>
                            </div>
                            Accedi con Google
                        </td>
                        <td>
                            <div>
                                <rp:OpenIdButton  runat="server" ImageUrl="~/images/yahoo.png" Text="Accedi con Yahoo!"
                                    ID="yahooLoginButton" Identifier="https://me.yahoo.com/" 
                                    OnLoggedIn="OpenIdLogin_LoggedIn" OnLoggingIn="OpenIdLogin_LoggingIn" />
                            </div>
                            Accedi con Yahoo!
                        </td>
                    </tr>
                </table>
                <div>
                    <div class="LoginBox">
                        <rp:OpenIdLogin ID="OpenIdLogin" runat="server" ButtonText="Accedi �" ButtonToolTip="Effettua l'accesso"
                            RegisterUrl = "http://openid.virgilio.it/openid/home/login.shtml"
                            CanceledText="Login annullata." ExamplePrefix="Esempio:" FailedMessageText="Login fallita: {0}"
                            RegisterText="Non hai un OpenId? Creane uno!" RegisterToolTip="Registrati adesso per ottenere un OpenID gratuito con Virgilio."
                            RememberMeText="Ricordami" RequestBirthDate="Require" RequestCountry="Require"
                            RequestEmail="Require" RequestFullName="Require" RequestGender="Require" RequestLanguage="Require"
                            RequestNickname="Require" RequestPostalCode="Require" RequestTimeZone="Require"
                            RequiredText="Prima inserisci un indirizzo OpenID." UriFormatText="Indirizzo OpenID invalido."
                            ExampleUrl="http://tuo.nome.myopenid.com" OnLoggedIn="OpenIdLogin_LoggedIn">
                          
                        </rp:OpenIdLogin>
                    </div>
                </div>
            </div>
        </div>
        <div id="fb-root">
        </div>

        <script type="text/javascript" src="http://connect.facebook.net/en_US/all.js"></script>

        <script type="text/javascript">
            FB.init({ appId: 'your app id', status: true, cookie: true, xfbml: true });
            FB.Event.subscribe('auth.login', function(response) {
                if (response.session) {
                    // A user has logged in, and a new cookie has been saved
                    alert('true');
                } else {
                    // The user has logged out, and the cookie has been cleared
                    alert('false');
                }
            });

            function FBLogin() {
                FB.login(function(response) {
                    if (response.session) {
                        // user successfully logged in
                        alert('true');
                    } else {
                        alert('false');
                    }
                });
            }
        </script>

    </div>
</asp:Content>
