<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Map.aspx.cs" Inherits="Map" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xmlns:v="urn:schemas-microsoft-com:vml">
<head runat="server">
    <base target="_top" />
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title runat="server"></title>

    <script src="https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false"></script>

    <link href="/StyleSheet.css" type="text/css" rel="stylesheet"/>
</head>
<body style="margin: 0px;">
    <form id="form1" runat="server" target="_self">
    <asp:Panel runat="server" ID="ChooseRoute" Visible="false">
        <asp:FileUpload ID="FileUploadGpx" runat="server" onchange="form.submit();" />
    </asp:Panel>
    <div id="MapContainer" style="margin-left: 0px; margin-right: 0px; margin-top: 0px;
        margin-bottom: 0px;">
        <div id="gmap_div" style="width: 100%; height: 100%; margin: 0px; margin-right: 12px;
            background-color: #F0F0F0; float: left; overflow: hidden;">
            <p align="center" style="font: 10px Arial;">
                Attendere il caricamento della mappa prego...</p>
        </div>
        <div id="gv_legend_container" style="display: none;">
            <table id="gv_legend_table" style="position: relative; filter: alpha(opacity=95);
                -moz-opacity: 0.95; opacity: 0.95; background: #ffffff;" cellpadding="0" cellspacing="0"
                border="0">
                <tr>
                    <td>
                        <div id="gv_legend_handle" align="center" style="height: 6px; max-height: 6px; background: #CCCCCC;
                            border-left: 1px solid #999999; border-top: 1px solid #EEEEEE; border-right: 1px solid #999999;
                            padding: 0px; cursor: move;">
                            <!-- -->
                        </div>
                        <div id="gv_legend" align="left" style="line-height: 13px; border: solid #000000 1px;
                            background: #FFFFFF; padding: 4px; font: 11px Arial;">
                            <div id="gv_legend_header" style="padding-bottom: 2px;">
                                <b>Altitudine (m)</b></div>
                            <%GenerateLegendItems();%>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="gv_tracklist_tooltip" class="gv_tracklist_tooltip" style="background-color: #FFFFFF;
            border: 1px solid #CCCCCC; padding: 2px; font: 11px Arial; display: none;">
        </div>
        <!-- the following is the "floating" marker list; the "static" version is below -->
        <div id="gv_marker_list_container" style="display: none;">
            <table id="gv_marker_list_table" style="position: relative; filter: alpha(opacity=95);
                -moz-opacity: 0.95; opacity: 0.95;" cellspacing="0" cellpadding="0" border="0">
                <tr>
                    <td>
                        <div id="gv_marker_list_handle" align="center" style="height: 6px; max-height: 6px;
                            background: #CCCCCC; border-left: 1px solid #999999; border-top: 1px solid #EEEEEE;
                            border-right: 1px solid #999999; padding: 0px; cursor: move;">
                            <!-- -->
                        </div>
                        <div id="gv_marker_list" align="left" class="gv_marker_list" style="overflow: auto;
                            background: #FFFFFF; border: solid #666666 1px; padding: 4px;">
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="gv_marker_list_static" align="left" class="gv_marker_list" style="width: 160px;
            overflow: auto; float: left; display: none;">
        </div>
        <div id="gv_clear_margins" style="height: 0px; clear: both;">
            <!-- clear the "float" -->
        </div>
    </div>
    <% 
        GenerateTrack();
        GenerateMarkers();
    %>

    <script type="text/javascript" src="../Script/gpsvisualizer.js">
    </script>

    <script type="text/javascript">
        this.mapReady = function() {
            addMarkers();
            addTracks();
        }
    </script>

    </form>
</body>
</html>
