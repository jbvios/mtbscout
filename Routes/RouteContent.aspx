<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RouteContent.aspx.cs" Inherits="Map" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xmlns:v="urn:schemas-microsoft-com:vml">
<head runat="server">
    <base target="_top" />
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title runat="server">Percorsi</title>
    <script src="https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false"></script>
    <link href="/StyleSheet.css" type="text/css" rel="stylesheet" />
</head>
<body style="margin: 0px;">
    <div style="margin-left: 0px; margin-right: 0px; margin-top: 0px; margin-bottom: 0px;">
        <div id="gmap_div" style="width: 100%; height: 100%; margin: 0px; margin-right: 12px;
            background-color: #F0F0F0; float: left; overflow: hidden;">
            <p align="center" style="font: 10px Arial;">
                Attendere il caricamento della mappa prego...</p>
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
    <!-- begin GPS Visualizer setup script (must come after maps.google.com code) -->
    <style type="text/css">
        /* Put any custom style definitions here (e.g., .gv_marker_info_window, .gv_marker_list_item, .gv_tooltip, .gv_label, etc.) */.gv_label
        {
            filter: alpha(opacity=80);
            -moz-opacity: 0.8;
            opacity: 0.8;
            background: #333333;
            border: 1px solid black;
            padding: 1px;
            font: 9px Verdana,sans-serif;
            color: white;
            font-weight: normal;
        }
    </style>
    <% 
        GenerateTrack();
        GenerateMarkers();
    %>

    <script type="text/javascript" src="../Script/gpsvisualizer.js">
    </script>

    <script type="text/javascript">
        this.mapReady = function() {
        addMarkers();
    }
        
    </script>

</body>
</html>
