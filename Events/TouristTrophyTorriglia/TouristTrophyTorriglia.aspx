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
            padding: 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPanel" runat="Server">
    <div id="ContentPanel" class="ContentPanel">
        <h1>
            Tourist Trophy Torriglia</h1>
        <h2>
            Domenica 16 giugno 2013</h2>
        <h3>
            <a href="Tourist Trophy Torriglia.pdf" title="Classifica" target="classifica">Classifica</a></h3>
        <h3>Il podio</h3>
        <img style="width:800px; height:600px" src="podio.jpg" alt="podio" />
        <h3>
            Il percorso</h3>
        <p>
            Dal centro di Torriglia (zona piscine) si percorre un breve tratto di asfalto fino
            a Marzano; si prosegue in direzione Scabbiabella sempre su asfalto fino ad imboccare,
            all&#39;altezza del centro abitato di Fricciaro, il sentiero a destra che sale ripido
            lungo il versante nord del monte Spigo; si incontrano alcuni tratti non pedalabilli,
            quindi si imbocca il sentiero in discesa che conduce alla cappella di Panteca. Da
            qui, su carrareccia, si scende sulla strada asfaltata che porta a Donetta, in corrispondenza
            del centro abitato di Porcarezze.
        </p>
        <p>
            Si prosegue su asfalto fino a Donetta quindi, oltrepassato il centro abitato e giunti
            all&#39;incrocio con la SP15, si imbocca i sentiero di cresta che conduce sopra
            la galleria di Buffalora prima (divertente discesa) e alle pendici del monte Lavagnola
            poi; è questo un impegnativo sentiero in salita, in particolare nel tratto terminale,
            ma interamente pedalabile se si escludono alcuni piccoli tratti in caso di fondo
            bagnato.</p>
        <p>
            Si percorre un breve segmento di Alta Via, quindi la si abbandona per imboccare
            il sentiero in discesa che presto diviene una carrareccia molto ripida che vi farà
            scaldare animo e freni, giungendo così sulla strada asfaltata nei pressi di Obbi.
            Si attraversa la strada e si imbocca quasi subito il sentiero che scende nel rio
            sottostante Torriglia, per poi risalire in quota fino all&#39;arrivo.</p>
        <p>
            A causa delle caratteristiche orografiche della zona il percorso, seppure non lungo,
            presenta alcuni tratti tecnici sia in salita sia in discesa; non aspettatevi quindi
            un tracciato interamente &#39;flow&#39;, ma siate certi che vi potrete immergere
            in quei gradevoli paesaggi naturalistici che hanno reso celebre, a buon diritto,
            la montagna dei Genovesi.</p>
        <h2>
            Costo iscrizione: 12 euro - Casco obbligatorio - Non occorre essere tesserati</h2>
        <iframe src="map.html" style="width: 800px; height: 400px;"></iframe>
        <p class="centered">
            <a title="Scarica tracciato GPS" href="torriglia.zip">Scarica tracciato GPS</a></p>
        <img src="locandina.jpg" />
        <a target="genoabike" title="Genoa Bike" href="http://www.genoabike.com">
            <img class="logo" alt="Genoa Bike" src="genoabike.gif" /></a> <a target="trala" title="Tra l'antola e il mare"
                href="http://www.tralantolaeilmare.org/">
                <img class="logo" alt="Tra l'antola e il mare" src="http://www.tralantolaeilmare.org/wp-content/uploads/2013/05/cropped-narcisi111.jpg" /></a>
    </div>
</asp:Content>
