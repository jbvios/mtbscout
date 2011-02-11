<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default"
    MasterPageFile="~/MasterPage.master" %>

<%@ Register Src="~/ImageRandomizer.ascx" TagName="ImageRandomizer" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Mountain Bike Group Scout</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPanel" runat="Server">
    <div id="ContentPanel" class="ContentPanel">
        <div class="MainCenter">
            <uc3:imagerandomizer id="MainImage" alternatetext="Montoggio" runat="server" imagefolder="~/Home" />
        </div>
    </div>
</asp:Content>
