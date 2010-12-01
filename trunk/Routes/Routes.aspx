<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/MasterPage.master"
    CodeFile="Routes.aspx.cs" Inherits="Routes" %>

<%@ Register Src="../HorizontalSpot.ascx" TagName="Spot" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<title>Percorsi</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPanel" runat="Server">
	<div id="ContentPanel" class="ContentPanel">
        <h1>
            Percorsi
        </h1>
        <p>
            La descrizione dei percorsi non ha pretese di esaustività, le condizioni dei tracciati
            possono variare in funzione di variabili climatiche o ambientali non conoscibili
            a priori.</p>
        <p>
            Affrontare sentieri di montagna in bicicletta presenta sempre una componente di
            rischio, si raccomanda pertanto costante prudenza e un equipaggiamento adeguato
            alla situazione, comprensivo quanto meno di casco, occhiali e una dose di buon senso
            e capacità di valutare i propri limiti.</p>
        <p>
            Il gruppo declina ogni responsabilità per danni a persone o cose che possano verificarsi
            a carico di chi affronta i percorsi descritti nella presente sezione.
        </p>
       
        <uc1:Spot ID="Spot1" runat="server" />
        <h1 style="margin-bottom:5px;">
            Mappa dei percorsi</h1>
        <a style="display:block; margin-bottom: 10px;" href="RouteContent.aspx" target="routes" title="Visualizza in un'altra pagina">
            (Visualizza in un'altra pagina)
        </a>
        <iframe style="width: 100%; height: 600px;" scrolling="no" frameborder="yes" noresize="noresize"
            src="RouteContent.aspx"></iframe>
    </div>
</asp:Content>
