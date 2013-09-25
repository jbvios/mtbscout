
function Size(w, h) {
	this.W = w;
	this.H = h;
}
function setLocation(element, x, y) {
	var style = element.style;
	style.position = 'absolute';
	style.left = x + "px";
	style.top = y + "px";
}

function animateNewsBanner() {
	var banner = document.getElementById('NewsBanner');
	if (!banner)
		return;
	banner.style.visibility = "visible";
	banner.style.display = "block";
	banner.style.position = "absolute";

	banner.style.zIndex = 100;

	adjustBannerSize(10);
}
function getWindowSize() {
	var myWidth = 0, myHeight = 0;
	if (typeof (window.innerWidth) == 'number') {
		//Non-IE
		myWidth = window.innerWidth;
		myHeight = window.innerHeight;
	} else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
		//IE 6+ in 'standards compliant mode'
		myWidth = document.documentElement.clientWidth;
		myHeight = document.documentElement.clientHeight;
	} else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
		//IE 4 compatible
		myWidth = document.body.clientWidth;
		myHeight = document.body.clientHeight;
	}

	return new Size(myWidth, myHeight);
}
function adjustBannerSize(size) {
	var banner = document.getElementById('NewsBanner');
	if (!banner)
		return;
	var wSize = getWindowSize();
	var w = wSize.W;
	var h = wSize.H;
	banner.style.height = size + "px";
	
	banner.style.top = ((h - size) / 2) + "px";
	banner.style.position = "fixed";
	var newSize = size + 20;
	if (newSize < getMaxBannerSize() && newSize < w && newSize < h) {
		size += 20;
		setTimeout("adjustBannerSize(" + size + ")", 5);
	}
}
function closeBanner() {
	var banner = document.getElementById('NewsBanner');
	banner.style.visibility = "hidden";
	banner.style.display = "none";

}

function getMaxBannerSize() {
	var banner = document.getElementById('BannerImage');
	if (!banner)
		return;
	return (banner.offsetWidth);
}
function Init() {
	animateNewsBanner();
}
function onResize() {
	adjustBannerSize(getMaxBannerSize());
}
window.onresize = onResize;
