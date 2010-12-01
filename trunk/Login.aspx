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
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPanel" runat="Server">
	<div id="ContentPanel" class="ContentPanel">
		<div class="box">
			<rp:OpenIdLogin ID="OpenIdLogin1" runat="server" ButtonText="Accedi »" ButtonToolTip="Effettua l'accesso"
				CanceledText="Login annullata." ExamplePrefix="Esempio:" FailedMessageText="Login fallita: {0}"
				RegisterText="Registrati" RegisterToolTip="Registrati adesso per ottenere un OpenID gratuito con MyOpenID."
				RememberMeText="Ricordami" RequestBirthDate="Require" RequestCountry="Require"
				RequestEmail="Require" RequestFullName="Require" RequestGender="Require" RequestLanguage="Require"
				RequestNickname="Require" RequestPostalCode="Require" RequestTimeZone="Require"
				RequiredText="Prima inserisci un indirizzo OpenID." 
				UriFormatText="Indirizzo OpenID invalido." 
				ExampleUrl="http://tuo.nome.myopenid.com" onloggedin="OpenIdLogin1_LoggedIn1">
				
			</rp:OpenIdLogin></div>
			
	</div>
</asp:Content>
