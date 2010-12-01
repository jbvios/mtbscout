<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default"
	StylesheetTheme="" %>

<%@ Register Src="~/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Src="~/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<%@ Register Src="~/ImageRandomizer.ascx" TagName="ImageRandomizer" TagPrefix="uc3" %>
<%@ Register Src="~/Menu.ascx" TagName="Menu" TagPrefix="uc4" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta name="verify-v1" content="Nwe1nXK2MlEJeEtCV06S4BtJRdWKbB2qHqxWPPO/o1E=" />
	<link href="StyleSheet.css" rel="stylesheet" type="text/css" />
	<title>Mountain Bike Group Scout</title>

	<!--script type="text/javascript" src="script/snow.js"></script-->
	<script type="text/javascript" src="script/HomeScript.js"></script>

	<script type="text/javascript" src="script/CommonScript.js"></script>

</head>
<body onload="Init();" style="background-image: none; height: auto;">
	<form id="Logo" runat="server">
	<!--<div id="NewsBanner" class="NewsBanner" >
		<div style="position: absolute; width: 98%; left: 1%; top: 5px">
			<img alt="Chiudi" title="Chiudi" onclick="closeBanner();" src="Images/Close.png"
				style="width: 20px; height: 20px; float: right;" />
		</div>
		<a href="http://www.bikersteamlivellato.it/index.php?option=com_content&view=article&id=49&Itemid=29" target="_blank">
		<img id = "BannerImage" border="0" alt="" src="Images/MTBLivellato.JPG" style="width: 100%; height: 100%" />
		</a>
	</div>-->
	<uc1:Header ID="Header1" runat="server" ShowAds="false" />
	<div class="MainCenter">
		<uc3:ImageRandomizer ID="MainImage" AlternateText="Montoggio" runat="server" ImageFolder="~/Home" />
	</div>
	<uc4:Menu ID="Menu1" runat="server" />
	<uc2:Footer ID="Footer1" runat="server" BackColor="Transparent" Color="#0066FF" Top="0px" />
	</form>
</body>
</html>
