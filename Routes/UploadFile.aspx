﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadFile.aspx.cs" Inherits="Routes_UploadFile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../StyleSheet.css" type="text/css" />

    <script type="text/javascript">
        function onFileSelected(input) {

            if (input.value.length > 0) {
                document.getElementById("waitImage").style.display = "block";
                document.forms[0].submit();

            }
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="waitImage" style="position: absolute; width: 100%; height: 100%; z-index: 10; left: 0px;
        top: 0px; background-image: url('../Images/wait.gif'); background-position: center;
        background-repeat: no-repeat; background-color: White; display: none;" title="Caricamento immagini in corso, attentere prego...">
    </div>
    <p style="text-align: justify;">
        Premi il pulsante sotto per aggiungere un'immagine
    </p>
    <div>
        <asp:FileUpload ID="file_upload" runat="server" name="file_upload" type="file"
            accept="image/jpg" onchange="onFileSelected(this);" />
    </div>
    </form>
</body>
</html>
