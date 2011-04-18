<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Events.aspx.cs" MasterPageFile="~/MasterPage.master"
    Inherits="Events" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Eventi</title>
    <style type="text/css">
        .Event
        {
            font-size: large;
            padding-top: 20px;
            padding-left: 20px;
            padding-right: 20px;
        }
        .Event td
        {
            padding: 10px;
            text-align: justify;
            vertical-align: text-top;
        }
        .DateColumn
        {
            width: 283px;
        }
        .EventColumn
        {
            width: 600px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPanel" runat="Server">
    <div id="ContentPanel" class="ContentPanel">
        <h1>
            Eventi</h1>
        <div class="Event">
            <table class="Event">
            <tr>
                    <td class="DateColumn">
                        <a href="Enduro2011/Enduro2011.aspx">Domenica 8 maggio 2011</a>
                    </td>
                    <td class="EventColumn">
                        Prima Gara di Enduro MTB
                    </td>
                </tr> 
                <tr>
                    <td class="DateColumn">
                        <a href="http://www.genoabike.com/media/manifestazioni/genoacup/main/genoacup00.html">Domenica 1 maggio 2011</a>
                    </td>
                    <td class="EventColumn">
                        Prima Tappa Coppa Italia Giovanile 2011
                    </td>
                </tr>
                <tr>
                    <td class="DateColumn">
                        <a href="CoppaItalia2010/CoppaItalia2010.aspx">Sabato 1 e domenica 2 maggio 2010</a>
                    </td>
                    <td class="EventColumn">
                        Prima Tappa Coppa Italia Giovanile MTB Cross Country
                    </td>
                </tr>
                <tr>
                <td class="DateColumn">
                    <a href="Antola2009/Antola2009.aspx">Sabato 22 agosto 2009</a>
                </td>
                <td class="EventColumn">
                    Escursione in mountain bike sul Monte Antola
                </td>
                </tr>
                <tr>
                    <td class="DateColumn">
                        <a href="Campionato2009/Campionato2009.aspx">Sabato 11 e domenica 12 luglio 2009</a>
                    </td>
                    <td class="EventColumn">
                        Campionato Italiano Giovanile MTB Cross Country
                    </td>
                </tr>
                <tr>
                    <td class="DateColumn">
                        <a href="RadunoOtt2008/RadunoOtt2008.aspx">Domenica 19 ottobre 2008</a>
                    </td>
                    <td class="EventColumn">
                        Pedalata Ecologica &quot;Anello dei Fieschi&quot;
                    </td>
                </tr>
                <tr>
                    <td class="DateColumn">
                        <a href="XCSet2008/XCSet2008.aspx">Domenica 21 settembre 2008</a>
                    </td>
                    <td class="EventColumn">
                        Gara Cross Country &quot;Secondo Trofeo Giacomazzi&quot;
                        <br />
                        Terza prova del Giro della Provincia di Genova
                    </td>
                </tr>
                <tr>
                    <td class="DateColumn">
                        <a id="PDFXC" runat="server" title="Scarica volantino">Domenica 17 giugno 2007</a>
                    </td>
                    <td class="EventColumn">
                        Gara Cross Country &quot;Primo Trofeo Giacomazzi&quot;
                        <br />
                        Valida per il Campionato Provinciale FCI
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
