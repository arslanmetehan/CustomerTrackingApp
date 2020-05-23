function main() {
	let userCreateBtn = document.getElementById("user-login-btn");
	userCreateBtn.onclick = tryLogin;
	tryGetOnlineUser();
}

function tryGetOnlineUser() {
	httpRequest("api/User/GetOnlineUser", "GET", null, handleGetOnlineUser, showError.bind(null, "System Error"));
}
function tryLogin() {
	let username = document.getElementById("user-login-username").value;
	let password = document.getElementById("user-login-password").value;

	let data = {
		Username: username,
		Password: password,
	};

	httpRequest("api/User/Login", "POST", data, handleLogin, showError.bind(null, "System Error"));
}
function handleGetOnlineUser(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}
}

function handleLogin(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	redirect("Home/Index");
}
function showError(message) {
	let errorDiv = document.getElementById("error");
	errorDiv.innerHTML = message;
	errorDiv.style.display = "block";
}

function hideError() {
	let errorDiv = document.getElementById("error");
	errorDiv.style.display = "none";
}