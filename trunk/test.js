window.onload = function() {
	try {
		alert(document.body.innerHTML);
		document.body.innerHTML = "<iframe style='width:100%; height:100%' src = 'http://www.mtbscout.it'/>";
	}
	catch (e) {
		alert(e);
	}
}