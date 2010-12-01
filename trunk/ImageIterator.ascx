<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ImageIterator.ascx.cs"
	Inherits="ImageIterator" %>
<%@ Register src="PageCounter.ascx" tagname="PageCounter" tagprefix="uc1" %>
<%@ Register src="HorizontalSpot.ascx" tagname="Spot" tagprefix="uc2" %>
    <uc2:Spot ID="Spot1" runat="server" />

<h3 style="margin-top: 30px;" runat="server" id="ImagesTitle">
	
	Galleria fotografica</h3>
<asp:UpdatePanel ID="ImagesPanel" runat="server" UpdateMode="Conditional" RenderMode="Block">
	<ContentTemplate>
		<input type="hidden" runat="server" id="Start" value="0" />
		<asp:Panel ID="PagesUp" runat="server">
			<asp:LinkButton ID="Previous" runat="server" Visible="false" OnClick="Previous_Click">Foto precedenti</asp:LinkButton>
			<uc1:PageCounter ID="PageCounterUp" runat="server"  />
			</asp:Panel>
		<asp:Table ID="ImagesTable" runat="server">
		</asp:Table>
		<asp:Panel ID="PagesDown" runat="server">
			<asp:LinkButton ID="Next" runat="server" Visible="false" OnClick="Next_Click">Foto successive</asp:LinkButton>
			<uc1:PageCounter ID="PageCounterDown" runat="server" />
		</asp:Panel>
	</ContentTemplate>
</asp:UpdatePanel>
