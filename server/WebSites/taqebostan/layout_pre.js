var browser = new detectBrowser();
function detectBrowser() {
	this.isKHTML = false;
	this.isGecko = false;
	this.isMSIE = false;
	this.isOpera = false;
	this.ver = null;
	this.platform = navigator.platform;
	this.engine = null;
	
	var browser = navigator.userAgent;
	
	if (browser.indexOf('KHTML') > -1) {
		this.isKHTML = true;
		this.engine = 'KHTML';
		return;
	}
	if (browser.indexOf('Gecko') > -1) {
		this.isGecko = true;
		this.engine = 'Gecko';
		return;
	}
	if (browser.indexOf('MSIE') > -1) {
		this.isMSIE = true;
		var pos1 = browser.indexOf('MSIE') + 5;
		var pos2 = browser.indexOf(';', pos1);
		this.ver = Number(browser.substring(pos1, pos2));
		this.engine = 'MSIE';
		return;
	}
	if (browser.indexOf('Opera') > -1) {
		this.isOpera = true;
		this.engine = 'Opera';
		return;
	}
}
function getPageScroll() {
	var yScroll;
	if (self.pageYOffset) {
		yScroll = self.pageYOffset;
	} else if (document.documentElement && document.documentElement.scrollTop) {
		yScroll = document.documentElement.scrollTop;
	} else if (document.body) {
		yScroll = document.body.scrollTop;
	}
	arrayPageScroll = new Array('',yScroll);
	return arrayPageScroll;
}

function getPageSize() {
	var xScroll, yScroll;
	if (window.innerHeight && window.scrollMaxY) {
		xScroll = document.body.scrollWidth;
		yScroll = window.innerHeight + window.scrollMaxY;
	} else if (document.body.scrollHeight > document.body.offsetHeight) {
		xScroll = document.body.scrollWidth;
		yScroll = document.body.scrollHeight;
	} else {
		xScroll = document.body.offsetWidth;
		yScroll = document.body.offsetHeight;
	}
	var windowWidth, windowHeight;
	if (self.innerHeight) {
		windowWidth = self.innerWidth;
		windowHeight = self.innerHeight;
	} else if (document.documentElement && document.documentElement.clientHeight) {
		windowWidth = document.documentElement.clientWidth;
		windowHeight = document.documentElement.clientHeight;
	} else if (document.body) {
		windowWidth = document.body.clientWidth;
		windowHeight = document.body.clientHeight;
	}	
	if(yScroll < windowHeight) {
		pageHeight = windowHeight;
	} else { 
		pageHeight = yScroll;
	}
	if(xScroll < windowWidth) {
		pageWidth = windowWidth;
	} else {
		pageWidth = xScroll;
	}
	arrayPageSize = new Array(pageWidth, pageHeight, windowWidth, windowHeight);
	return arrayPageSize;
}

function fixPreLoader() {
	var arrayPageSize = getPageSize();
	var arrayPageScroll = getPageScroll();
	var obj = document.getElementById('dvPreLoader');
	obj.style.height = arrayPageSize[1] + 'px';
	obj.style.width = arrayPageSize[0] - 99 + 'px';
	var objLoadingImage = document.getElementById('loadingImage');
	objLoadingImage.style.display = 'block';
	objLoadingImage.style.top = Math.round(arrayPageScroll[1] + (arrayPageSize[3] - 35 - objLoadingImage.height) / 2) + 'px';
	objLoadingImage.style.left = Math.round((arrayPageSize[0] - 20 - objLoadingImage.width) / 2) + 'px';
	monitorPreLoader(0, 0, null, null);
}

function monitorPreLoader(xSpeed, ySpeed, xDir, yDir) {
	var arrayPageSize = getPageSize();
	var arrayPageScroll = getPageScroll();
	var objLoadingImage = document.getElementById('loadingImage');
	var top = Math.round(arrayPageScroll[1] + (arrayPageSize[3] - 35 - objLoadingImage.height) / 2);
	var left = Math.round((arrayPageSize[0] - 20 - objLoadingImage.width) / 2);
	var objTop = parseInt(objLoadingImage.style.top);
	var objLeft = parseInt(objLoadingImage.style.left);
	var maxSpeed = 3;
	var speedRate = 0.1;
	if (xSpeed < maxSpeed)
		xSpeed += speedRate;
	if (ySpeed < maxSpeed)
		ySpeed += speedRate;
	if (objTop < top) {
		if (yDir == 'D' || yDir == null)
			objLoadingImage.style.top = objTop + ySpeed + 'px';
		else
			objLoadingImage.style.top = top + 'px';
		yDir = 'D';
	}
	else if (objTop > top) {
		if (yDir == 'U' || yDir == null)
			objLoadingImage.style.top = objTop - ySpeed + 'px';
		else
			objLoadingImage.style.top = top + 'px';
		yDir = 'U';
	}
	else {
		ySpeed = 0;
		yDir = null;
	}
	if (objLeft < left) {
		if (xDir == 'R' || xDir == null)
			objLoadingImage.style.left = objLeft + xSpeed + 'px';
		else
			objLoadingImage.style.left = left + 'px';
		xDir = 'R';
	}
	else if (objLeft > left) {
		if (xDir == 'L' || xDir == null)
			objLoadingImage.style.left = objLeft - xSpeed + 'px';
		else
			objLoadingImage.style.left = left + 'px';
		xDir = 'L';
	}
	else {
		xSpeed = 0;
		xDir = null;
	}
	xs = xSpeed;
	ys = ySpeed;
	xd = xDir;
	yd = yDir;
	window.setTimeout('monitorPreLoader(xs, ys, xd, yd);', 9);
}

function hidePreloader() {
	var obj = document.getElementById('dvPreLoader');
	obj.style.visibility = 'hidden';
	obj.style.height = 0 + 'px';
	obj.style.width = 0 + 'px';
	obj.style.zIndex = 0;
	var html = document.getElementsByTagName("html");
	if (browser.isMSIE) {
		html[0].style.filter = '';
	}
	html[0].style.cursor = 'default';
	var objLoadingImage = document.getElementById('loadingImage');
	objLoadingImage.style.visibility = 'hidden';
}
window.onload = function() {
	hidePreloader();
};