<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadFile.aspx.cs" Inherits="Routes_UploadFile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<link href="../StyleSheet.css" type="text/css" />
</head>
<body>
	<form id="form1" runat="server">
	<p>
		Premi il pulsante sotto per aggiungere immagini. Se il tuo browser lo supporta,
		puoi selezionare più immagini contemporaneamente tenendo premuto il tasto CTRL
	</p>
	<div>
		<asp:FileUpload ID="file_upload" runat="server" name="file_upload" type="file" multiple="" accept="image/jpg"
			onchange="form.submit();" />
	</div>
	</form>
</body>
</html>
