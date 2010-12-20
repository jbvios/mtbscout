<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
	CodeFile="RouteRankDetail.aspx.cs" Inherits="Routes_RouteRankDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<title>Dettaglio voti</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPanel" runat="Server">
	<div id="ContentPanel" class="ContentPanel">
		<h1>
			Dettaglio voti per il percorso
		</h1>
		<h2 runat="Server" id="RouteTitle">
		</h2>
		<asp:Repeater ID="Repeater1" runat="server">
			<HeaderTemplate>
				<table border="1" width="100%">
					<tr>
						<th>
							Nome
						</th>
						<th>
							Cognome
						</th>
						<th>
							Nickname
						</th>
						<th>
							Voto
						</th>
					</tr>
			</HeaderTemplate>
			<ItemTemplate>
				<tr>
					<td>
						<%#DataBinder.Eval(Container.DataItem, "Name")%>
					</td>
					<td>
						<%#DataBinder.Eval(Container.DataItem, "Surname")%>
					</td>
					<td>
						<%#DataBinder.Eval(Container.DataItem, "Nickname")%>
					</td>
					<td>
						<%#DataBinder.Eval(Container.DataItem, "Rank")%>
					</td>
				</tr>
			</ItemTemplate>
			<FooterTemplate>
				</table>
			</FooterTemplate>
		</asp:Repeater>
	</div>
</asp:Content>
