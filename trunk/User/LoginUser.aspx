<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="LoginUser.aspx.cs" Inherits="User_LoginUser" %>

<%@ Register Src="Login.ascx" TagName="Login" TagPrefix="uc1" %>
<%@ Register Src="User.ascx" TagName="User" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Accesso registrato</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPanel" runat="Server">
    <div id="ContentPanel" class="ContentPanel">
        <uc1:Login ID="Login1" runat="server" />
        <uc2:User ID="User1" runat="server" />
    </div>
</asp:Content>
