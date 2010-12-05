<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Menu.ascx.cs" Inherits="Menu" %>
<link href="StyleSheet.css" rel="stylesheet" type="text/css" />
<div id="MenuContainer" class="Horiz">
    <ul style="z-index: 1000">
        <li><a runat="server" id="AWhoWeAre" href="~/whoweare/whoweare.aspx" title="Chi siamo">
            <img id="WhoWeAre" runat="server" src="~/Images/WhoWeAre.png" alt="Chi siamo" onmouseover="hoverImage(this);"
                onmouseout="normalImage(this);" /></a></li>
        <li><a runat="server" id="AEvents" href="~/Events/Events.aspx" title="Eventi">
            <img id="Events" runat="server" src="~/Images/Events.png" alt="Eventi" onmouseover="hoverImage(this);"
                onmouseout="normalImage(this);" /></a> </li>
        <li><a runat="server" id="ARoutes" href="~/Routes/Routes.aspx" title="Percorsi">
            <img id="Routes" runat="server" src="~/Images/Routes.png" alt="Percorsi" onmouseover="hoverImage(this);"
                onmouseout="normalImage(this);" /></a> </li>
        <li><a runat="server" id="ALinks" href="~/Links.aspx" title="Links utili">
            <img id="Links" runat="server" src="~/Images/Links.png" alt="Links utili" onmouseover="hoverImage(this);"
                onmouseout="normalImage(this);" /></a> </li>
        <li><a runat="server" id="ABlog" href="~/Blog.aspx" title="Blog">
            <img id="Blog" runat="server" src="~/Images/Blog.png" alt="Blog" onmouseover="hoverImage(this);"
                onmouseout="normalImage(this);" /></a> </li>
        <li><a runat="server" id="AUser" href="~/User/User.aspx" title="Pannello utente">
            <img id="User" runat="server" src="~/Images/ContactUs.png" alt="Pannello utente" onmouseover="hoverImage(this);"
                onmouseout="normalImage(this);" /></a></li>
    </ul>
</div>
