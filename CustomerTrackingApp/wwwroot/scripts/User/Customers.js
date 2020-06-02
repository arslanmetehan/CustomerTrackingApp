function main() {
	let customerCreateBtn = document.getElementById("customer-create-btn");
	customerCreateBtn.onclick = tryInsertCustomer;

	
	tryGetOnlineUser();
	createMenu();
}
function redirectTo(url) {
	redirect(""+url+"");
}
function createMenu() {
	/*let menu = document.getElementById("side-bar");
	let home = document.createElement("button");
	home.class = "menu-item";
	home.innerHTML = "Home";
	menu.appendChild(home);
	home.onclick = redirectTo.bind(null, "Home/Index");*/
	let home = document.getElementById("home-btn");
	home.setAttribute("href", "Index");
	
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
	tryGetCustomers();
}
function tryGetCustomers() {
	httpRequest("api/Customer/GetCustomers", "GET", null, handleGetCustomers, showError.bind(null, "System Error"));
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
	
}
function handleGetCustomers(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}
	page.customers = response.Data;
	pagination();
	tryGetCustomersByPage(1);
	openUserModal();
}
function pagination() {
	var customersCount = page.customers.length;
	var pageCount = customersCount / 5;
	let paginationDiv = document.getElementById("pagination-box");
	page.pageNumbers = [];
	if (customersCount == null || customersCount == undefined || customersCount == 0) {
		let pageNumber = document.createElement("button");
		pageNumber.className = "page-number";
		pageNumber.id = "page-" + 1;
		pageNumber.innerHTML = 1;
		pageNumber.value = 1;
		paginationDiv.appendChild(pageNumber);
		page.pageNumbers.push(1);
		pageNumber.onclick = tryGetCustomersByPage.bind(null, pageNumber.value)
		return;
    }
	for (i = 1; i < pageCount+1; i++) {

		let pageNumber = document.createElement("button");
		pageNumber.className = "page-number";
		pageNumber.id = "page-" + i;
		pageNumber.innerHTML = i;
		pageNumber.value = i;
		paginationDiv.appendChild(pageNumber);
		page.pageNumbers.push(i);
		pageNumber.onclick = tryGetCustomersByPage.bind(null, pageNumber.value)
		
    }

}
function tryGetCustomersByPage(pageNo) {

	classNameOrganizer(pageNo);
	httpRequest("api/Customer/GetCustomersByPageNo/?pageNo=" + pageNo, "GET", null, handleGetCustomersByPage, showError.bind(null, "System Error"));
}
function classNameOrganizer(pageNo)
{
	for (i = 1; i < page.pageNumbers.length+1; i++) {
		let pageNumberBtn = document.getElementById("page-" + i);
		pageNumberBtn.className = "page-number other-pages";
    }
	let pageNumberBtn = document.getElementById("page-" + pageNo);
	pageNumberBtn.className = "page-number current-page";
}
/*function appendUserForPage(pageNo) {
	let userList = document.getElementById("user-list");
	userList.innerHTML = "";
	if (pageNo == 1) {
		for (i = 0; i < 5; i++) {
			let user = page.users[i]
			appendUser(user);
		}
		return;
	}
	for (i = 5 * pageNo - 1; i < 5 * pageNo; i++) {
		let user = page.users[i]
		appendUser(user);
    }
}*/
function handleGetCustomersByPage(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	let customerList = document.getElementById('customers');
	customerList.innerHTML = "";
	
	page.customers = response.Data;
	for (i = 0; i < page.customers.length; i++) {
		let customer = page.customers[i];
		appendCustomer(customer);
	}
}
function tryInsertCustomer() {
	let fullname = document.getElementById("user-create-fullname").value;
	let phone = document.getElementById("user-create-phone").value;

	if (fullname == "" ||
		phone == "") {
		let message = "Mandatory fields must be filled";
		return showError(message);

	}
	if (!parseInt(phone)) {
		return showError("Phone number can not be letter");
    }
	let data = {
		FullName: fullname,
		Phone: parseInt(phone),

	};

	httpRequest("api/Customer/CreateCustomer", "POST", data, handleInsertCustomer, showError.bind(null, "System Error"));
}
function redirectToCustomers() {
	redirect("Home/Customers");
}
function handleInsertCustomer(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	let customer = response.Data;

	appendCustomer(customer);
	redirectToCustomers();
}
function appendCustomer(customer) {
	let userProfileLink = generateHref("User/CustomerProfile/##customer.Id##");
	let userTemplate = '<div class="clearfix user-line-box" id="user-id-##customer.Id##">';
	userTemplate += '<a class="user-info profile-btn" href="' + userProfileLink + '">##customer.FullName##</a>';
	userTemplate += '<div class="user-info">##customer.Phone##</div>';
	userTemplate += '</div>';

	let userHtmlString = userTemplate
		.split("##customer.Id##").join(customer.Id)//.replace("##user.Id##", userModel.Id)
		.split("##customer.FullName##").join(customer.FullName)//.replace("##user.Username##", userModel.Username)
		.split("##customer.Phone##").join(customer.Phone)//.replace("##user.Email##", userModel.Email)

	let userHtml = toDom(userHtmlString);

	let userListDiv = document.getElementById("customers");
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