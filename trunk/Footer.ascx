<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Footer.ascx.cs" Inherits="Footer" %>
<asp:Panel id ="MainFooterPanel" runat="server" CssClass="FooterContainer">
<br />
<div style="text-align: center">
Sei il visitatore numero: <% Response.Write(GetSessionNumber()); %> - Visitatori attualmente connessi: <% Response.Write(Helper.GetActiveSessionCount()); %>
</div>

<div style="text-align: center">
<a href="mailto:info@mtbscout.it" >info@mtbscout.it</a>
</div>
</asp:Panel>