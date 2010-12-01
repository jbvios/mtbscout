<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Events.aspx.cs" MasterPageFile="~/MasterPage.master"
	Inherits="Events" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<title>Eventi</title>
	<style type="text/css">
		.Event
		{
			font-size: large;
			padding-top: 20px;
			padding-left: 20px;
			padding-right: 20px;
		}
		.Event td
		{
			padding-top: 10px;
			padding-bottom: 10px;
			text-align: justify;
			vertical-align: text-top;
		}
		.DateColumn
		{
			width: 283px;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPanel" runat="Server">
	<div id="ContentPanel" class="ContentPanel">
		<h1>
			Eventi</h1>
		<div class="Event">
			<table class="Event">
				<td class="DateColumn">
						<a href="Antola2009/Antola2009.aspx">Sabato 22 agosto 2009</a>
					</td>
					<td>
						Escursione in mountain bike sul Monte Antola
					</td>
				</tr>
				<tr>
					<td class="DateColumn">
						<a href="Campionato2009/Campionato2009.aspx">Sabato 11 e domenica 12 luglio 2009</a>
					</td>
					<td>
						Campionato Italiano Giovanile MTB Cross Country
					</td>
				</tr>
				<tr>
					<td class="DateColumn">
						<a href="RadunoOtt2008/RadunoOtt2008.aspx">Domenica 19 ottobre 2008</a>
					</td>
					<td>
						Pedalata Ecologica &quot;Anello dei Fieschi&quot;
					</td>
				</tr>
				<tr>
					<td class="DateColumn">
						<a href="XCSet2008/XCSet2008.aspx">Domenica 21 settembre 2008</a>
					</td>
					<td>
						Gara Cross Country &quot;Secondo Trofeo Giacomazzi&quot;
						<br />
						Terza prova del Giro della Provincia di Genova
					</td>
				</tr>
				<tr>
					<td class="DateColumn">
						<a id="PDFXC" runat="server" title="Scarica volantino">Domenica 17 giugno 2007</a>
					</td>
					<td>
						Gara Cross Country &quot;Primo Trofeo Giacomazzi&quot;
						<br />
						Valida per il Campionato Provinciale FCI
					</td>
				</tr>
			</table>
		</div>
	</div>
</asp:Content>
