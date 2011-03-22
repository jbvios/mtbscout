<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default"
	MasterPageFile="~/MasterPage.master" %>
<%@ Register Src="HorizontalSpot.ascx" TagName="Spot" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<title>Mountain Bike Group Scout</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPanel" runat="Server">
	<div id="ContentPanel" class="ContentPanel">
		<div style="padding-top: 20px;">
		<uc1:Spot ID="Spot2" runat="server" />
			<div class="widget">
				<div style="padding:20px;">
					<span style="padding-top: 30px;">Guarda i nostri percorsi registrati, scarica la traccia
						GPS e buon divertimento!</span>
					<asp:UpdatePanel ID="routePreview" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
						<ContentTemplate>
							<div id="ImageLayer">
								<asp:HyperLink runat="server" ID="A1">
									<asp:Image ID="RandomImage1" runat="server" Style="left: 0px;" /></asp:HyperLink>
								<asp:HyperLink runat="server" ID="A2">
									<asp:Image ID="RandomImage2" runat="server" Style="left: 200px;" /></asp:HyperLink>
								<asp:HyperLink runat="server" ID="A3">
									<asp:Image ID="RandomImage3" runat="server" Style="left: 400px;" /></asp:HyperLink>
								<input type="submit" id="reloadImages" />
							</div>
						</ContentTemplate>
					</asp:UpdatePanel>
					<br />
					<span id="RouteTitle" style="padding-top: 30px;"></span>
				</div>
			</div>
		<uc1:Spot ID="Spot1" runat="server" />
		</div>
	</div>
</asp:Content>
