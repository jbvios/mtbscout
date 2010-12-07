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
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPanel" runat="Server">
	<div id="ContentPanel" class="ContentPanel">
		<div class="box">
			<h1>
				Inserisci le tue credenziali</h1>
			<div class="LoginBox">
				<rp:OpenIdLogin ID="OpenIdLogin" runat="server" ButtonText="Accedi »" ButtonToolTip="Effettua l'accesso"
					CanceledText="Login annullata." ExamplePrefix="Esempio:" FailedMessageText="Login fallita: {0}"
					RegisterText="Registrati" RegisterToolTip="Registrati adesso per ottenere un OpenID gratuito con MyOpenID."
					RememberMeText="Ricordami" RequestBirthDate="Require" RequestCountry="Require"
					RequestEmail="Require" RequestFullName="Require" RequestGender="Require" RequestLanguage="Require"
					RequestNickname="Require" RequestPostalCode="Require" RequestTimeZone="Require"
					RequiredText="Prima inserisci un indirizzo OpenID." UriFormatText="Indirizzo OpenID invalido."
					ExampleUrl="http://tuo.nome.myopenid.com" OnLoggedIn="OpenIdLogin_LoggedIn">
				</rp:OpenIdLogin>
			</div>
			<div style="padding: 20px;">
				<rp:OpenIdButton runat="server" ImageUrl="~/images/yahoo.png" Text="Usa il tuo OpenId Yahoo!"
					ID="yahooLoginButton" Identifier="https://me.yahoo.com/" OnLoggedIn="OpenIdLogin_LoggedIn" />
			</div>
			<div style="padding: 20px;" title = "Usa le tue credenziali Facebook">
			
				<fb:login-button></fb:login-button></div>
			<p>
				Pensi di non avere un OpenId? Forse non è così: molti portali in cui sei probabilmente
				già registrato offrono questo servizio; di seguito ti offriamo alcuni esempi, <a
					href="http://www.openid.it/dove/" target="_blank">qui</a> trovi ulteriori informazioni.</p>
			<p>
				Ma cosè OpenId? E&#39; un servizio che ti permette di usare lo stesso nome e password
				che usi nel tuo portale preferito anche per effettuare l&#39;accesso ad altri siti;
				in questo modo si evita il proliferare di password con tutti i problemi di sicurezza
				connessi alla gestione delle stesse.</p>
			<div style="text-align: left; padding-left: 30px; padding-right: 30px;">
				<table>
					<tbody>
						<tr>
							<td>
								<a href="http://google.com">
									<img alt="Google" src="/images/google.png" /></a>
							</td>
							<td>
								<p>
									Recupera <a target="_blank" href="http://google.com/profiles/me">qui</a> il tuo
									indirizzo di profilo Google: ti verrà richiesto di autenticarti con le credenziali
									Google, poi verrai reindirizzato alla tua pagina di profilo; a quel punto, copia
									l&#39;indirizzo della pagina e inseriscilo nella casella dell&#39;OpenId</p>
							</td>
						</tr>
						<tr>
							<td>
								<a href="http://livejournal.com">
									<img alt="LiveJournal" src="/images/livejournal.png" /></a>
							</td>
							<td>
								<p>
									Inserisci “<strong>nomeutente</strong>.livejournal.com”</p>
							</td>
						</tr>
						<tr>
							<td>
								<a href="http://blogger.com">
									<img alt="Blogger" src="/images/blogger.png" /></a>
							</td>
							<td>
								<p>
									Inserisci il tuo indirizzo di BLOG: “<strong>nomeblog</strong>.blogspot.com”</p>
							</td>
						</tr>
						<tr>
							<td>
								<a href="http://myspace.com">
									<img alt="MySpace" src="/images/myspace.png"></a>
							</td>
							<td>
								<p>
									Inserisci “www.myspace.com/<b>nomeutente</b>”</p>
							</td>
						</tr>
						<tr>
							<td>
								<a href="http://wordpress.com">
									<img alt="Wordpress" src="/images/wordpress.png"></a>
							</td>
							<td>
								<p>
									Inserisci il tuo indirizzo WordPress.com, per esempio: “<strong>nomeutente</strong>.wordpress.com”</p>
							</td>
						</tr>
						<tr>
							<td>
								<a href="http://aol.com">
									<img alt="AOL" src="/images/aol.png"></a>
							</td>
							<td>
								<p>
									Inserisci “openid.aol.com/<strong>nomeutente</strong>”</p>
							</td>
						</tr>
					</tbody>
				</table>
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

</asp:Content>
