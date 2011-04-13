<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
	CodeFile="Enduro2011.aspx.cs" Inherits="Enduro2011" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<title>Prima Discesa a cronometro</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPanel" runat="Server">
	<div id="ContentPanel" class="ContentPanel">
		<h1>
			Prima Gara di Enduro MTB</h1>
		<h2>
			Domenica 8 maggio 2011</h2>
		<h3>
			Organizzata da Genova Ciclo Sport 1989</h3>
		<p>
			Gara di discesa a cronometro su percorsi stile &quot;Super Enduro&quot;; tre discese 
			tecniche dopo altrettanti percorsi di trasferimento che attraverseranno paesaggi 
			dal panorama mozzafiato! In questo <a href="/Public/Routes/Banca_Castello/Banca_Castello.aspx">link</a> trovate la descrizione del percorso (con 
			video in soggettiva) che ripercorre la terza discesa, quella più spaccabraccia!</p>
		<p>
			Le iscrizioni si terranno la mattina dell&#39;evento presso il centro Sportivo di 
			Montoggio a partire dalle 8.30.</p>
		<iframe style="width: 900px; height: 900px" src="map.html"></iframe>
		<br />
		Tracciato di gara<br />
		<br />
		<iframe id="FBLike" runat="server" frameborder="0" name="I1" scrolling="no" style="border: none;
			width: 330px; height: 50px"></iframe>
		<div class="ImageAndDesc">
			<img id="ProfileImage" runat="server" alt="Profilo altimetrico" src="profile.png" />
			<div>
				Profilo altimetrico</div>
		</div>
		<table style="width: 60%; margin-left: auto; margin-right: auto;">
			<tr>
				<td style="width: 50%">
					<div class="ImageAndDesc">
						<asp:HyperLink ID="HyperLinkToGps" NavigateUrl="~/Events/Enduro2011/track.zip" runat="server"
							ToolTip="Tracciato GPS">
							<asp:Image ID="Image1" runat="server" ImageUrl="~/Routes/gps.jpg" AlternateText="Tracciato GPS"
								BorderWidth="1px" />
						</asp:HyperLink>
						<div>
							Tracciato GPS</div>
					</div>
				</td>
			</tr>
		</table>
	</div>
</asp:Content>
