function hoverImage(el) {
	el.className = "HoverIteratorImage";
}
function normalImage(el) {
	el.className = "IteratorImage";
}
function hoverDownloadImage(el) {
	el.style.width = "35px";
	el.style.height = "35px";
}
function normalDownloadImage(el) {
	el.style.width = "30px";
	el.style.height = "30px";
}
function hoverDeleteImage(el) {
	el.style.width = "25px";
	el.style.height = "25px";
}
function normalDeleteImage(el) {
	el.style.width = "20px";
	el.style.height = "20px";
}


function CalcDeltaX(deltaY, radius) {
	var angle = Math.asin(deltaY / radius);

	return radius * Math.cos(angle);
}
function Round(id, radius, step) {
	var el = document.getElementById(id);
	if (el == null)
		return;

	var w = el.offsetWidth;
	var currEl = el;
	var deltay = step;
	var deltax = true;
	while (deltay <= radius) {
		deltax = CalcDeltaX(deltay, radius);
		deltay += step;

		var divUp = document.createElement("div");
		divUp.className = el.className;
		divUp.style.padding = "0px";
		divUp.style.marginTop = "0px";
		divUp.style.marginBottom = "0px";
		divUp.style.overflow = "hidden";
		divUp.style.height = step + "px";
		divUp.style.width = (w - Math.round(((radius - deltax) * 2))) + "px";
		divUp.style.zIndex = -1;
		currEl.parentNode.insertBefore(divUp, currEl);
		currEl = divUp;

		//var divDown = divUp.cloneNode(true);
		//el.parentNode.appendChild(divDown);

	}
}

function InitPage() {

}
function moveRouteImage(div, left) {
	left = left - 2;
	if (left < -340) {
		document.getElementById("reloadImages").click();	
		return;
	}
	div.style.left = left + "px";
	setTimeout(function() { moveRouteImage(div, left); }, 5);
}