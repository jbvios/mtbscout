<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Campionato2012.aspx.cs" Inherits="Campionato2012" %>

<%@ Register Src="~/ImageIterator.ascx" TagName="ImageIterator" TagPrefix="uc1" %>
<%@ Register Src="../../DownloadGpsTrack.ascx" TagName="DownloadGpsTrack" TagPrefix="uc2" %>
<%@ Register Src="../../Video.ascx" TagName="Video" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Terza Tappa Campionato Italiano Giovanile di Società - MTB Cross Country</title>
    <style type="text/css">
        .Indented
        {
            padding-left: 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPanel" runat="Server">
    <div id="ContentPanel" class="ContentPanel">
        <h1>
            Terza Tappa Campionato Italiano Giovanile di Società
        </h1>
        <h1>
            MTB Cross Country</h1>
        <h2>
            Sabato 12 e domenica 13 maggio 2012</h2>
        <p>
            Come ogni anno, grande festa con il cross country a Montoggio sul nostro collaudato tracciato
            di 4 Km. Dopo un periodo molto piovoso, il tempo ci ha graziati regalandoci una bella giornata all'insegna del divertimento; 
            i torrenti colmi d&#39;acqua hanno protestato un po&#39;, ma alla fine si è riusciti 
            ugualmente a creare i guadi, anche se abbiamo dovuto rinunciare a quello sul 
            Laccio.</p>
        <p>
            Un grazie a Michela per le belle foto e a tutti quelli che hanno collaborato per 
            il percorso e l&#39;organizzazione.</p>
        <iframe src="map.html" style="height:600px; width:800px"></iframe>
        
        <p style="text-align:center">Tracciato del percorso - <a href="track.zip" >scarica la traccia GPS</a></p>
        <uc3:Video ID="Video2" runat="server" VideoUrl="..\CoppaItalia2010\CoppaItalia2010.flv"
            PreviewUrl="..\CoppaItalia2010\CoppaItalia2010.jpg" Title="Video del percorso di gara (tratto nel bosco - Coppa Italia 2010))" />
        <uc3:Video ID="Video1" runat="server" VideoUrl="..\Campionato2009\Campionato2009.flv"
            PreviewUrl="..\Campionato2009\Campionato2009.jpg" Title="Video del percorso di gara (Campionato Italiano 2009)" />
    <uc1:ImageIterator ID="ImageIterator1" runat="server" />
    </div>
</asp:Content>
