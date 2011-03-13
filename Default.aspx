<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default"
	MasterPageFile="~/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<title>Mountain Bike Group Scout</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPanel" runat="Server">
	<div id="ContentPanel" class="ContentPanel">
		<div style="padding-top: 20px;">
			<asp:UpdatePanel ID="routePreview" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
				<ContentTemplate>
					<div class="widget">
						<div>
							<div id="ImageLayer">
								<asp:HyperLink runat="server" ID="A1">
									<asp:Image ID="RandomImage1" runat="server" Style="left: 0px;" /></asp:HyperLink>
								<asp:HyperLink runat="server" ID="A2">
									<asp:Image ID="RandomImage2" runat="server" Style="left: 340px;" /></asp:HyperLink>
								<asp:HyperLink runat="server" ID="A3">
									<asp:Image ID="RandomImage3" runat="server" Style="left: 780px;" /></asp:HyperLink>
								<input type="submit" id="reloadImages" />
							</div>
						</div>
						
		
					</div>
				
				</ContentTemplate>
			</asp:UpdatePanel>
			</div>
	</div>
</asp:Content>
