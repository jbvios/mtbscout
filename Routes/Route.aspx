<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Route.aspx.cs" Inherits="Routes_Route" %>

<%@ Register Src="~/ImageIterator.ascx" TagName="ImageIterator" TagPrefix="uc1" %>
<%@ Register Src="~/DownloadGpsTrack.ascx" TagName="DownloadGpsTrack" TagPrefix="uc2" %>
<%@ Register Src="~/RouteHeader.ascx" TagName="RouteHeader" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPanel" runat="Server">
    <div id="ContentPanel" class="ContentPanel">
        <uc4:RouteHeader ID="RouteHeader1" runat="server" />
        <%
            
            foreach (string s in DescriptionParagraphs)
            {%>
               <p>
               <%= s.Trim()%>
               </p>
            <%}
             %>
        <uc2:DownloadGpsTrack ID="DownloadGpsTrack1" runat="server" />
        <uc1:ImageIterator ID="ImageIterator1" runat="server" />
    </div>
</asp:Content>
