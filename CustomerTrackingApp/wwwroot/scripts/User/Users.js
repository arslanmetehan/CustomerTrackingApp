function main() {
	let userCreateBtn = document.getElementById("user-create-btn");
	userCreateBtn.onclick = tryInsertUser;

	tryGetUsers();
}

function tryGetUsers() {
	httpRequest("api/User/GetActiveUsers", "GET", null, handleGetUsers, showError.bind(null, "System Error"));
}
function openUserModal() {
	var modal = document.getElementById("user-modal");
	var btn = document.getElementById("user-modal-btn");
	var span = document.getElementsByClassName("close")[0];
	btn.onclick = function () {
		modal.style.display = "block";
	}
	span.onclick = function () {
		modal.style.display = "none";
	}
	window.onclick = function (event) {
		if (event.target == modal) {
			modal.style.display = "none";
		}
	}
	selectUser();

}
function selectUser() {

	let userSelect = document.getElementById("select-user-type");
	var opt = userSelect.options[userSelect.selectedIndex];

	let managerSelectDiv = document.getElementById("manager");
	let managerSelect = document.getElementById("select-manager");
	let userTypeTitle = document.getElementById("user-type-title");
	for (i = 0; i < page.managers.length; i++) {
		let manager = page.managers[i];
		var opt = document.createElement('option');
		opt.appendChild(document.createTextNode(manager.Username));
		opt.value = manager.Id;
		managerSelect.appendChild(opt); 
    }
	if (opt.value == 0) {
		
		managerSelectDiv.style.display = "none";
		managerSelect.style.display = "none";
		userTypeTitle.innerHTML = "Creating an Administrator";
	}
	else if (opt.value == 1)
	{
		managerSelectDiv.style.display = "none";
		managerSelect.style.display = "none";
		userTypeTitle.innerHTML = "Creating a Manager";
    }
	else
	{
		managerSelectDiv.style.display = "block";
		managerSelect.style.display = "block";
		userTypeTitle.innerHTML = "Creating an Employee";
	}
	document.getElementById("chose-type-btn").addEventListener('click', selectUser);
}
function handleGetUsers(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}
	page.users = response.Data;
	for (i = 0; i < page.users.length; i++) {
		let user = page.users[i];
		if (user.Type == 1) {
			page.managers.push(user);
        }
	}
	let userModelBtn = document.getElementById("user-modal-btn");
	userModelBtn.onclick = openUserModal;
}
function tryInsertUser() {
	var type = document.getElementById("select-user-type");
	var userType = type.options[type.selectedIndex];
	var userTypeValue = userType.value;

	var manager = document.getElementById("select-manager");
	managerId = manager.options[manager.selectedIndex];
	var managerIdValue = managerId.value;
	if (userTypeValue == 0 || userTypeValue == 1)
	{
		managerIdValue = 0;
	}
	var gender = document.getElementById("select-gender");
	var genderType = gender.options[gender.selectedIndex];
	var genderValue = genderType.value;

	let username = document.getElementById("user-create-username").value;
	let fullname = document.getElementById("user-create-fullname").value;
	let email = document.getElementById("user-create-email").value;
	let password = document.getElementById("user-create-password").value;
	let birthyear = document.getElementById("user-create-birthyear").value;
	let phone = document.getElementById("user-create-phone").value;


	let data = {
		Username: username,
		Fullname: fullname,
		Email: email,
		Password: password,
		Gender: genderValue,
		BirthYear: parseInt(birthYear),
		Phone: parseInt(phone),
		Manager: managerIdValue,
		Type: userTypeValue,

	};

	httpRequest("api/User/CreateUser", "POST", data, handleInsertUser, showError.bind(null, "System Error"));
}

function handleInsertUser(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	let user = response.Data;
	appendUser(user);
}

function appendUser(user) {
	let userTemplate = '<div id="user-id-##user.Id##">';
	userTemplate += '<div>##user.Username##';
	userTemplate += '<div>##user.Fullname##';
	userTemplate += '<div>##user.Email##';
	userTemplate += '<div>##user.Gender##';
	userTemplate += '<div>##user.Phone##';
	userTemplate += '<div>##user.Manager##';
	userTemplate += '<div>##user.Type##';
	userTemplate += '<div>##user.BirthYear##';
	userTemplate += '</div>';

	let userHtmlString = userTemplate
		.split("##user.Id##").join(user.Id)//.replace("##user.Id##", userModel.Id)
		.split("##user.Username##").join(user.Username)//.replace("##user.Username##", userModel.Username)
		.split("##user.Fullname##").join(user.Fullname)//.replace("##user.Username##", userModel.Username)
		.split("##user.Email##").join(user.Email)//.replace("##user.Email##", userModel.Email)
		.split("##user.Phone##").join(user.Phone)//.replace("##user.Email##", userModel.Email)
		.split("##user.Manager##").join(user.Manager)//.replace("##user.Email##", userModel.Email)
		.split("##user.Type##").join(user.Type)//.replace("##user.Email##", userModel.Email)
		.split("##user.BirthYear##").join(user.BirthYear)//.replace("##user.Email##", userModel.Email)

	let userHtml = toDom(userHtmlString);

	let userListDiv = document.getElementById("user-list");
	userListDiv.appendChild(userHtml);

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