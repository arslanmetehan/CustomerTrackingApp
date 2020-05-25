function main() {
	let userCreateBtn = document.getElementById("user-create-btn");
	userCreateBtn.onclick = tryInsertUser;

	tryGetUsers();
	tryGetOnlineUser();
}
function tryGetOnlineUser() {
	httpRequest("api/User/GetOnlineUser", "GET", null, handleGetOnlineUser, showError.bind(null, "System Error"));
}
function handleGetOnlineUser(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}
	page.onlineUser = response.Data;
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
	if (page.onlineUser.Type != 0) {
		userSelect.removeChild(userSelect.options[0]);
		userSelect.removeChild(userSelect.options[0]);
    }
	var userOpt = userSelect.options[userSelect.selectedIndex];

	let managerSelectDiv = document.getElementById("manager");
	let managerSelect = document.getElementById("select-manager");
	let userTypeTitle = document.getElementById("user-type-title");

	if (page.onlineUser.Type == 1) {	
		var opt = document.createElement("option");
		opt.appendChild(document.createTextNode(page.onlineUser.Username));
		opt.value = page.onlineUser.Id;
		managerSelect.appendChild(opt);
	}
	else {
		
		managerSelect.querySelectorAll('*').forEach(n => n.remove());
		for (i = 0; i < page.managers.length; i++) {
			let manager = page.managers[i];
			var managerOpt = document.createElement("option");
			managerOpt.appendChild(document.createTextNode(manager.Username));
			managerOpt.value = manager.Id;
			managerSelect.appendChild(managerOpt);
		}
    }

	if (userOpt.value == 0) {

		managerSelectDiv.style.display = "none";
		managerSelect.style.display = "none";
		userTypeTitle.innerHTML = "Creating an Administrator";
	}
	else if (userOpt.value == 1) {
		managerSelectDiv.style.display = "none";
		managerSelect.style.display = "none";
		userTypeTitle.innerHTML = "Creating a Manager";
	}
	else {
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
	page.managers = [];
	page.users = response.Data;
	for (i = 0; i < page.users.length; i++) {
		let user = page.users[i];
		if (user.Type == 1) {
			page.managers.push(user);
		}
		if (user.Type == 0) {
			user.Type = "Admin";
		}
		else if (user.Type == 1) {
			user.Type = "Manager";
		}
		else {
			user.Type = "Employee";
		}
		if (user.Gender == 0) {
			user.Gender = "Female";
		}
		else {
			user.Gender = "Male";
		}
		appendUser(user);
	}
	let userModelBtn = document.getElementById("user-modal-btn");
	userModelBtn.onclick = openUserModal;
}
function tryInsertUser() {
	var type = document.getElementById("select-user-type");
	var userType = type.options[type.selectedIndex];
	var userTypeValue = userType.value;

	var manager = document.getElementById("select-manager");
	var managerId = manager.options[manager.selectedIndex];
	var managerIdValue = managerId.value;
	if (userTypeValue == 0 || userTypeValue == 1) {
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

	if (username == "" ||
		fullname == "" ||
		email == "" ||
		password == "" ||
		birthyear == "" ||
		phone == "") {
		let message = "Mandatory fields must be filled";
		return showError(message);

    }
	let data = {
		Username: username,
		FullName: fullname,
		Email: email,
		Password: password,
		Gender: parseInt(genderValue),
		BirthYear: parseInt(birthyear),
		Phone: parseInt(phone),
		ManagerId: parseInt(managerIdValue),
		Type: parseInt(userTypeValue),

	};

	httpRequest("api/User/CreateUser", "POST", data, handleInsertUser, showError.bind(null, "System Error"));
}
function redirectToUsers() {
	redirect("Home/Users");
}
function handleInsertUser(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	let user = response.Data;

	appendUser(user);
	redirectToUsers();
}
function appendUser(user) {
	let userProfileLink = generateHref("User/Profile/##user.Id##");
	let userTemplate = '<div class="clearfix user-line-box" id="user-id-##user.Id##">';
	userTemplate += '<a class="user-info profile-btn" href="'+ userProfileLink +'">##user.Username##</a>';
	userTemplate += '<div class="user-info">##user.FullName##</div>';
	userTemplate += '<div class="user-info">##user.Email##</div>';
	userTemplate += '<div class="user-info">##user.Gender##</div>';
	userTemplate += '<div class="user-info">##user.Phone##</div>';
	userTemplate += '<div class="user-info">##user.Type##</div>';
	userTemplate += '<div class="user-info">##user.BirthYear##</div>';
	userTemplate += '</div>';

	let userHtmlString = userTemplate
		.split("##user.Id##").join(user.Id)//.replace("##user.Id##", userModel.Id)
		.split("##user.Username##").join(user.Username)//.replace("##user.Username##", userModel.Username)
		.split("##user.FullName##").join(user.FullName)//.replace("##user.Username##", userModel.Username)
		.split("##user.Email##").join(user.Email)//.replace("##user.Email##", userModel.Email)
		.split("##user.Gender##").join(user.Gender)//.replace("##user.Email##", userModel.Email)
		.split("##user.Phone##").join(user.Phone)//.replace("##user.Email##", userModel.Email)
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