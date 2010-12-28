<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadFile.aspx.cs" Inherits="Routes_UploadFile" %>

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
	<p style="text-align:justify;">
		Premi il pulsante sotto per aggiungere immagini. Se il tuo browser lo supporta,
		puoi selezionare più immagini contemporaneamente tenendo premuto il tasto CTRL
	</p>
	<div>
		<img id="waitImage" style="display: none; position:relative;top:-70px; margin-left: auto; margin-right: auto;" alt="Caricamento immagini in corso, attentere prego..."
			src="../Images/wait.gif" title="Caricamento immagini in corso, attentere prego..." />
		<asp:FileUpload ID="file_upload" runat="server" name="file_upload" type="file" multiple=""
			accept="image/jpg" onchange="onFileSelected(this);" />
	</div>
	</form>
</body>
</html>
