function httpRequest(url, method, data, onSuccess, onFailure) {
	let xhr;

	if (typeof XMLHttpRequest !== 'undefined') {
		xhr = new XMLHttpRequest();
	}
	else {
		var versions = ["MSXML2.XmlHttp.5.0",
			"MSXML2.XmlHttp.4.0",
			"MSXML2.XmlHttp.3.0",
			"MSXML2.XmlHttp.2.0",
			"Microsoft.XmlHttp"];

		for (var i = 0, len = versions.length; i < len; i++) {
			try {
				xhr = new ActiveXObject(versions[i]);
				break;
			}
			catch (e) { }
		}
	}

	function ensureReadiness() {
		if (xhr.readyState < 4)
			return;

		if (xhr.status !== 200)
			return;

		if (xhr.readyState === 4) {
			if (xhr.status !== 200) {
				onFailure(xhr);
			} else {
				let result = JSON.parse(xhr.responseText);
				onSuccess(result);
			}
		}

	}

	var userAgent = navigator.userAgent.toLowerCase();

	if (userAgent.indexOf("msie") != -1 && userAgent.indexOf("opera") == -1 && userAgent.indexOf("webtv") == -1) {
		xhr.onreadystatechange = ensureReadiness;
	}
	else {
		xhr.onload = function (e) {
			if (this.status == 200) {
				let result = JSON.parse(xhr.responseText);
				onSuccess(result);
			}
			else {
				onFailure(xhr);
			}
		};
	}

	method = method.toUpperCase();
	let validMethods = ['GET', 'POST', 'PUT', 'DELETE'];
	if (validMethods.indexOf(method) < 0) {
		console.error('invalid http method');
		return;
	}

	var baseUrl = getBaseUrl()

	xhr.open(method, baseUrl + url, true);

	if (data) {
		if (typeof data == 'object') {
			data = JSON.stringify(data);
		}

		xhr.setRequestHeader("Content-type", "application/json");
	}

	//var token = document.getElementsByName("__RequestVerificationToken")[0].value;
	//xhr.setRequestHeader("RequestVerificationToken", token);
	//xhr.setRequestHeader("X-XSRF-Token", token);

	xhr.send(data);
}

function getBaseUrl() {
	var browserUrl = window.location.href;
	var browserUrlArr = browserUrl.split("/");
	var baseUrl = browserUrlArr[0] + "//" + browserUrlArr[2] + "/";

	return baseUrl;
}

function redirect(url) {
	var baseUrl = getBaseUrl();

	window.location.href = baseUrl + url;
}
function generateHref(url) {
	var baseUrl = getBaseUrl();

	return baseUrl + url;
}