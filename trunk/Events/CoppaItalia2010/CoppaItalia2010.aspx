<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
	CodeFile="CoppaItalia2010.aspx.cs" Inherits="CoppaItalia2010" %>

<%@ Register Src="~/ImageIterator.ascx" TagName="ImageIterator" TagPrefix="uc1" %>
<%@ Register Src="../../DownloadGpsTrack.ascx" TagName="DownloadGpsTrack" TagPrefix="uc2" %>
<%@ Register Src="../../Video.ascx" TagName="Video" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<title>Prima Tappa Coppa Italia Giovanile MTB Cross Country</title>
	<style type="text/css">
		.Indented
		{
			padding-left: 20px;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPanel" runat="Server">
	<div id="ContentPanel" class="ContentPanel">
		<h1>
			Prima Tappa Coppa Italia Giovanile
		</h1>
		<h1>
			MTB Cross Country</h1>
		<h2>
			Sabato 1 e domenica 2 maggio 2010</h2>
		<p>
			Dopo la buona riuscita del Campionato Italiano dello scorso anno, il successo si
			ripete quest'anno con la prima tappa della Coppia Italia. Il percorso ricalca a
			grandi linee quello dell'anno precedente, con l&#39;aggiunta di due varianti tecniche. Ecco le classifiche:</p>
		<div id="menu">
			<p style="text-align:center;">
				<a href="Esordienti1Anno.pdf" target="_blank">Esordienti Primo anno M.<br/>
				</a><a href="EsordientiDonne1Anno.pdf" target="_blank">Esordienti Primo anno F.<br/>
				</a><a href="Esordienti2Anno.pdf" target="_blank">Esordienti Secondo anno M.<br/>
				</a><a href="EsordientiDonne2Anno.pdf" target="_blank">Esordienti Secondo anno F.<br/>
				</a><a href="Allievi1Anno.pdf" target="_blank">Allievi Primo anno M.<br/>
				</a><a href="AllieviDonne1Anno.pdf" target="_blank">Allievi Primo anno F.<br/>
				</a><a href="Allievi2Anno.pdf" target="_blank">Allievi Secondo anno M.<br/>
				</a><a href="AllieviDonne2Anno.pdf" target="_blank">Allievi Secondo anno F.<br/>
				</a><a href="Comitati.pdf" target="_blank">Comitati</a><br/>
				<br/>
			</p>
		</div>
		<p style="text-align: center;">
			<a href="http://www.genoabike.com/media/manifestazioni/genoacup/main/genoacup00.html"
				target="_blank">Vai al sito di Genoa Bike</a></p>
		<uc3:Video ID="Video2" runat="server" VideoUrl="CoppaItalia2010.flv" PreviewUrl="CoppaItalia2010.jpg"
			Title="Video del percorso di gara (tratto nel bosco)" />
		<uc3:Video ID="Video1" runat="server" VideoUrl="..\Campionato2009\Campionato2009.flv"
			PreviewUrl="..\Campionato2009\Campionato2009.jpg" Title="Video del percorso di gara (Campionato Italiano 2009)" />
		<uc3:Video ID="Video3" runat="server" VideoUrl="primocanale.flv" PreviewUrl="..\Campionato2009\Campionato2009.jpg"
			Title="Servizio al telegiornale" />
	</div>
</asp:Content>
