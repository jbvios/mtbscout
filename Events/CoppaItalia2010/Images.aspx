<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Images.aspx.cs" MasterPageFile="~/MasterPage.master"
	Inherits="Campionato2009_Images" %>

<%@ Register Src="~/ImageIterator.ascx" TagName="ImageIterator" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<title>Campionato Italiano Giovanile MTB Cross Country</title>
	
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPanel" runat="Server">
	<div id="ContentPanel" class="ContentPanel">
		<h1>
			Album Fotografico del Campionato Italiano Giovanile MTB Cross Country</h1>
		<h2>
			12 luglio 2009</h2>
		<h3>
			di Claudio Molini</h3>
		<h3>
			<a href="http://www.fotomolini.it" target="_blank" title="Foto Molini">
			<img src="../../Images/Fotomolini.gif" alt="Foto Molini" title="Foto Molini"/></a></h3>
		<p style="text-align: center">
			<a href="Campionato2009.aspx">Torna alla pagina precendente</a></p>
		<uc1:ImageIterator ID="ImageIterator" runat="server" />
		<p style="text-align: center">
			<a href="Campionato2009.aspx">Torna alla pagina precendente</a></p>
	</div>
</asp:Content>
