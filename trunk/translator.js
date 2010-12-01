
window.onload = function() { translatePage(); };

function onLoadFrame() {

	var content = this.iFrame.contentDocument.getElementById("bodyContent");
	content.value = document.body.innerHTML;
	var button = this.iFrame.contentDocument.getElementById("submitButton");
	button.click();

	document.body.style.visibility = "visible";
}

function translatePage() {
	var html = document.body.innerHTML;
	document.body.style.visibility = "hidden";

	this.iFrame = document.createElement('iframe');
	this.iFrame.style.visibility = "hidden";
	this.iFrame.setAttribute("src", "http://www.mtbscout.it/DataBag.htm");
	this.iFrame.setAttribute("onload", "onLoadFrame();");
	document.body.appendChild(this.iFrame);
}