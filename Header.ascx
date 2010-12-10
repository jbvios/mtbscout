<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Header.ascx.cs" Inherits="SiteHeader" %>
<div class="HeaderContainer">
    <a href="/Default.aspx" title="Vai alla pagina iniziale">
        <div id="SpotLeft" style="width: 180px; height: 150px; margin-top: 15px; float: left;
            display: inline;" runat="server">

            <script type="text/javascript"><!--
                google_ad_client = "pub-8836579110679228";
                /* 180x150, creato 30/12/08 */
                google_ad_slot = "3942976439";
                google_ad_width = 180;
                google_ad_height = 150;
//-->
            </script>

            <script type="text/javascript" src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
            </script>

        </div>
        <div id="SpotRight" style="width: 180px; height: 150px; margin-top: 15px; float: right;
            display: inline;" runat="server">

            <script type="text/javascript"><!--
                google_ad_client = "pub-8836579110679228";
                /* 180x150, creato 30/12/08 */
                google_ad_slot = "7865457186";
                google_ad_width = 180;
                google_ad_height = 150;
//-->
            </script>

            <script type="text/javascript" src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
            </script>

        </div>
        <asp:Image runat="server" AlternateText="MTB Group Scout" ImageUrl="~/Images/MtbScoutLogoNew.PNG" />
        <br />
        <asp:Image runat="server" AlternateText="MTB Group Scout" ImageUrl="~/Images/Title.png"
            Style="position: relative; top: -62px; margin-left: auto; margin-right: auto;
             position: relative;" />
    </a>
</div>
