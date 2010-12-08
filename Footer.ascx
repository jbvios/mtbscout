<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Footer.ascx.cs" Inherits="Footer" %>
<asp:Panel ID="MainFooterPanel" runat="server" CssClass="FooterContainer">
    <br />
    <div style="text-align: center">
        <span>
            <% Response.Write(GetUserString());%>Sei il visitatore numero:
            <% Response.Write(GetSessionNumber()); %>
            - Visitatori attualmente connessi:
            <% Response.Write(Helper.GetActiveSessionCount()); %></span>
    </div>
    <div style="text-align: center">
    <asp:LinkButton runat="server"  ID="Disconnect" onclick="Disconnect_Click">[Esci]</asp:LinkButton>
    </div>
    <div style="text-align: center">
        <a href="mailto:info@mtbscout.it">info@mtbscout.it</a>
    </div>
</asp:Panel>
