<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DownloadGpsTrack.ascx.cs"
	Inherits="DownloadGpsTrack" %>
	
<%@ Register Src="Donate.ascx" TagName="Donate" TagPrefix="uc1" %>
<uc1:Donate ID="Donate1" runat="server" />
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
	<img ID="ProfileImage" runat="server" alt="Profilo altimetrico" src=""/>
	<div>
		Profilo altimetrico</div>
		
</div>

<iframe id = "MeteoFrame" runat ="server" style="width: 550px; height: 250px;" scrolling="no" frameborder="yes" noresize="noresize"/>
<div>Previsioni Meteo</div>
