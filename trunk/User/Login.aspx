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
        .LoginButtons
        {
            width: 500px;
            text-align: center;
            margin-left: auto;
            margin-right: auto;
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
                non avere un OpenId? Forse non è così: molti portali in cui sei probabilmente già
                registrato offrono questo servizio; di seguito ti offriamo alcuni esempi, <a href="http://www.openid.it/dove/"
                    target="_blank">qui</a> trovi ulteriori informazioni.</p>
            <p>
                Ma cosè OpenId? E&#39; un servizio che ti permette di usare lo stesso nome e password
                che usi nel tuo portale preferito anche per effettuare l&#39;accesso ad altri siti;
                in questo modo si evita il proliferare di password con tutti i problemi di sicurezza
                connessi alla gestione delle stesse.
            </p>
            <p class="style1">
                <strong>MTBScout NON gestisce o conserva informazioni relative alle password.</strong></p>
            <div style="text-align: left; padding-left: 30px; padding-right: 30px;">
                <table class="LoginButtons">
                    <tr>
                        <td>
                            <div>
                                <rp:OpenIdButton runat="server" ImageUrl="~/images/google.png" Text="Accedi con Google"
                                    ID="googleLoginButton" Identifier="https://www.google.com/accounts/o8/id" OnLoggedIn="OpenIdLogin_LoggedIn"
                                    OnLoggingIn="OpenIdLogin_LoggingIn" />
                            </div>
                            Accedi con Google
                        </td>
                        <td>
                            <div style="width: 260px; height: 33px; margin-top: 10px;">
                                <div id="fb-root">
                                </div>

                                <script>
                                    function fbLogin() {
                                        FB.getLoginStatus(function(response) {
                                            if (response.status === 'connected') {
                                                var uid = response.authResponse.userID;
                                                var sep = (window.location.href.indexOf('?') == -1) ? '?' : '&';
                                                window.location.href = window.location.href + sep + 'fbId=' + encodeURIComponent(uid);
                                            }
                                        });
                                    }
                                    window.fbAsyncInit = function() {
                                        FB.init({
                                            appId: '268710583196873',
                                            status: true,
                                            cookie: true,
                                            xfbml: true,
                                            oauth: true
                                        });
                                        FB.Event.subscribe('auth.login',
                                        function(response) {
                                            fbLogin();
                                        });
                                    };
                                    (function(d) {
                                        var js, id = 'facebook-jssdk'; if (d.getElementById(id)) { return; }
                                        js = d.createElement('script'); js.id = id; js.async = true;
                                        js.src = "//connect.facebook.net/en_US/all.js";
                                        d.getElementsByTagName('head')[0].appendChild(js);
                                    } (document));
                                </script>

                                <div class="fb-login-button" title="Accedi con Facebook" onclick="fbLogin();">
                                    Facebook</div>
                            </div>
                            Accedi con Facebook
                        </td>
                        <td>
                            <div>
                                <rp:OpenIdButton runat="server" ImageUrl="~/images/yahoo.png" Text="Accedi con Yahoo!"
                                    ID="yahooLoginButton" Identifier="https://me.yahoo.com/" OnLoggedIn="OpenIdLogin_LoggedIn"
                                    OnLoggingIn="OpenIdLogin_LoggingIn" />
                            </div>
                            Accedi con Yahoo!
                        </td>
                    </tr>
                </table>
                <div>
                    <div class="LoginBox">
                        <rp:OpenIdLogin ID="OpenIdLogin" runat="server" ButtonText="Accedi »" ButtonToolTip="Effettua l'accesso"
                            CanceledText="Login annullata." ExamplePrefix="Esempio:" FailedMessageText="Login fallita: {0}"
                            RegisterText="Non hai un OpenId? Creane uno!" RegisterToolTip="Registrati adesso per ottenere un OpenID gratuito"
                            RequestEmail="Require" RequestFullName="Require" RequestGender="Require" RequestLanguage="Require"
                            RequestNickname="Require" RequestPostalCode="Require" RequestTimeZone="Require"
                            RequiredText="Prima inserisci un indirizzo OpenID." UriFormatText="Indirizzo OpenID invalido."
                            ExampleUrl="http://tuo.nome.myopenid.com" OnLoggedIn="OpenIdLogin_LoggedIn">
                        </rp:OpenIdLogin>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
