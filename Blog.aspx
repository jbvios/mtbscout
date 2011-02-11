<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
	CodeFile="Blog.aspx.cs" Inherits="Blog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<title>Blog</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPanel" runat="Server">
	<div id="ContentPanel" class="ContentPanel">
	<iframe style="margin-left:auto;margin-right:auto;padding-left:0px;padding-right:0px" id="blogFrame" name="blogframe" frameborder="0" vspace="0" hspace="0"
marginwidth="0" marginheight="0" scrolling="auto" noresize width="980px" height="1000px"></iframe>
	</div>
</asp:Content>
