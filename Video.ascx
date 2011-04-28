<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Video.ascx.cs" Inherits="Video" %>

<p style="text-align: center; margin-top: 10px; margin-bottom: 20px;">
<!-- START OF THE PLAYER EMBEDDING TO COPY-PASTE -->
<object id="player" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" name="player"
	width="<%Response.Write(VideoWidth);%>" height="<%Response.Write(VideoHeight);%>">
	<param name="movie" value="/video/player-viral.swf" />
	<param name="allowfullscreen" value="true" />
	<param name="allowscriptaccess" value="always" />
	<param name="flashvars" <%Response.Write(Value); %> />
	<object type="application/x-shockwave-flash" data="/video/player-viral.swf" width="<%Response.Write(VideoWidth);%>" height="<%Response.Write(VideoHeight);%>">
		<param name="movie" value="/video/player-viral.swf" />
		<param name="allowfullscreen" value="true" />
		<param name="allowscriptaccess" value="always" />
		<param name="flashvars" <%Response.Write(Value); %> />
		<p style="text-align:center"><a href="http://get.adobe.com/flashplayer">Scarica Flash</a> per vedere questo video.</p>
	</object>
</object>
<br />
			<%Response.Write(Title); %>
</p>
<!-- END OF THE PLAYER EMBEDDING -->
