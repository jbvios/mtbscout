<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
	CodeFile="Admin.aspx.cs" Inherits="Admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPanel" runat="Server">
	<div id="ContentPanel">
		<asp:TextBox ID="TextBoxQuery" runat="server" Height="159px" TextMode="MultiLine"
			Width="666px"></asp:TextBox>
		<br />
		<asp:TextBox ID="TextBoxResult" runat="server" Height="76px" TextMode="MultiLine"
			Width="666px"></asp:TextBox><br />
		<asp:Button ID="ButtonExecQuery" runat="server" OnClick="ButtonExecQuery_Click" Text="Esegui query" />
		<br />
		<asp:GridView style="margin-left:auto;margin-right:auto;" ID="GridViewSelectResults" runat="server">
		</asp:GridView>
	</div>
</asp:Content>
