<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DownloadGpsTrack.ascx.cs"
	Inherits="DownloadGpsTrack" %>
<%@ Register Src="Donate.ascx" TagName="Donate" TagPrefix="uc1" %>
<uc1:Donate ID="Donate1" runat="server" />
<br />
<iframe id="FBLike" runat="server" frameborder="0" name="I1" scrolling="no" style="border: none;
	width: 330px; height: 50px"></iframe>

<asp:UpdatePanel runat="server" ChildrenAsTriggers="true" UpdateMode="Always" ID="RankPanel">
	<ContentTemplate>
		<div class="ImageAndDesc">
			Dai il tuo voto a questo percorso
			<asp:RadioButtonList ID="Rank" runat="server" ToolTip="Dai il tuo voto a questo percorso"
				RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="True" CausesValidation="True"
				OnSelectedIndexChanged="Rank_SelectedIndexChanged">
				<asp:ListItem>1</asp:ListItem>
				<asp:ListItem>2</asp:ListItem>
				<asp:ListItem>3</asp:ListItem>
				<asp:ListItem>4</asp:ListItem>
				<asp:ListItem>5</asp:ListItem>
				<asp:ListItem>6</asp:ListItem>
				<asp:ListItem>7</asp:ListItem>
				<asp:ListItem>8</asp:ListItem>
				<asp:ListItem>9</asp:ListItem>
				<asp:ListItem>10</asp:ListItem>
			</asp:RadioButtonList>
		</div>
	</ContentTemplate>
</asp:UpdatePanel>

<table style="width: 60%; margin-left: auto; margin-right: auto;">
	<tr>
		<td style="width: 50%">
			<div class="ImageAndDesc">
				<asp:HyperLink ID="HyperLinkToGps" runat="server" ToolTip="Tracciato GPS">
					<asp:Image ID="Image1" runat="server" ImageUrl="~/Routes/gps.jpg" AlternateText="Tracciato GPS"
						BorderWidth="1px" />
				</asp:HyperLink>
				<div>
					Tracciato GPS</div>
			</div>
		</td>
		<td style="width: 50%">
			<div class="ImageAndDesc">
				<asp:HyperLink runat="server" ID="MapLink" Target="_blank" ToolTip="Visualizza mappa">
					<asp:Image ID="Image2" runat="server" ImageUrl="~/Routes/Map.jpg" AlternateText="Visualizza mappa"
						BorderWidth="1px" />
				</asp:HyperLink>
				<div>
					Visualizza Mappa</div>
			</div>
		</td>
	</tr>
</table>
<div class="ImageAndDesc">
	<img id="ProfileImage" runat="server" alt="Profilo altimetrico" src="" />
	<div>
		Profilo altimetrico</div>
</div>
<iframe id="MeteoFrame" runat="server" style="width: 550px; height: 250px;" scrolling="no"
	frameborder="yes" noresize="noresize" />
<div>
	Previsioni Meteo</div>
