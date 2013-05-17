<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="TouristTrophyTorriglia.aspx.cs" Inherits="Events_TouristTrophyTorriglia_TouristTrophyTorriglia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Tourist Trophy Torriglia</title>
    <style type="text/css">
        p.centered
        {
            text-align: center;
        }
        img.logo
        {
            height: 100px;
            border: none;
            padding:20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPanel" runat="Server">
    <div id="ContentPanel" class="ContentPanel">
        <h1>
            Tourist Trophy Torriglia</h1>
        <h2>
            Domenica 16 giugno 2013</h2>
             <p class="centered" style="font-size:larger">
            <a href="../../User/Subscriptions.aspx" title="Preiscrizioni">Preiscrizioni</a></p>
            <img src="locandina.jpg"/>
        <a target="genoabike" title="Genoa Bike" href="http://www.genoabike.com">
            <img class="logo" alt="Genoa Bike" src="genoabike.gif" /></a> <a target="trala" title="Tra l'antola e il mare"
                href="http://www.tralantolaeilmare.org/">
                <img class="logo" alt="Tra l'antola e il mare" src="http://www.tralantolaeilmare.org/wp-content/uploads/2013/05/cropped-narcisi111.jpg" /></a>
       
    </div>
</asp:Content>
