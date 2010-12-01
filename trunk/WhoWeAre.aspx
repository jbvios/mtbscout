<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WhoWeAre.aspx.cs" Inherits="WhoWeAre" %>
<%@ Register src="~/Header.ascx" tagname="Header" tagprefix="uc1" %>

<%@ Register src="~/Footer.ascx" tagname="Footer" tagprefix="uc2" %>

<%@ Register src="~/Menu.ascx" tagname="Menu" tagprefix="uc3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<link href="StyleSheet.css" rel="stylesheet" type="text/css" />
	<title>Chi siamo</title>
</head>
<body >
	<form id="form1" runat="server">
	
	<uc1:Header ID="Header1" runat="server" />
	<uc3:Menu ID="Menu1" runat="server" />
	<asp:Panel ID="ContentPanel" runat="server">
	<h1>Mountain Bike Group Scout</h1>
	<p>L&#39;organizzazione è fondata da un gruppo di amici accomunati dalla passione per la 
		Mountain Bike, vista come mezzo di comunione con la natura e occasione per 
		vivere momenti di serena libertà, lontano da strade trafficate e rumorose.</p>
		<p>
			L&#39;organizzazione, senza fini di lucro, si prefigge di diffondere la pratica di 
			questo sport da un lato, e la conoscenza del territorio dall&#39;altro, promuovendo 
			iniziative volte alla promozione turistica di quest&#39;angolo di entroterra ligure.</p>
			
			
			<iframe width="550" height="285" scrolling="no" frameborder="yes" noresize="noresize"
src="http://www.ilmeteo.it/script/meteo.php?id=free&citta=4459"></iframe>

	</asp:Panel>
	<uc2:Footer ID="Footer1" runat="server" />
	</form>
</body>
</html>
