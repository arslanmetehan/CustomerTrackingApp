function main() {
	tryGetOnlineUser();
	let purchaseCreateBtn = document.getElementById("purchase-create-btn");
	purchaseCreateBtn.onclick = tryInsertPurchase;

	let paymentCreateBtn = document.getElementById("payment-create-btn");
	paymentCreateBtn.onclick = tryInsertPayment;

	let returnCreateBtn = document.getElementById("return-create-btn");
	returnCreateBtn.onclick = tryInsertReturn;
	
	let msgBoxBtn = document.getElementById("message-box-btn");
	msgBoxBtn.onclick = clearMsg;

	let customerId = parseInt(document.getElementById("customer-id-input").value);
	page.customerId = customerId;

	tryGetLastActivityByCustomerId(customerId);
	tryGetActivitiesByCustomerId(customerId);
	ReplacingImage(customerId);
}
function getCustomerImg(customerId) {
	/*var img = new Image();
	img.src = '....images/customer-image-"' + customerId + '".jpg';
	var div = document.getElementById('customer-img');
	div.innerHTML += '<img src="' + img.src + '" />'; */

}
function ReplacingImage(customerId) {

	document.getElementById("customer-img").src = "../../images/customer-image-" + customerId+ ".jpg"

}
function clearMsg() {
	let msg = document.getElementById("message-box");
	msg.innerHTML = "";
	let msgDiv = document.getElementById("message-div");
	msgDiv.style.display = "none";
}
function tryGetLastActivityByCustomerId(customerId) {

	httpRequest("api/Customer/GetLastActivityByCustomerId/?customerId=" + customerId, "GET", null, handleGetLastActivity, showError.bind(null, "System Error"));
}
function tryGetActivitiesByCustomerId(customerId) {

	httpRequest("api/Customer/GetActivitiesByCustomerId/?customerId=" + customerId, "GET", null, handleGetActivities, showError.bind(null, "System Error"));
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
function handleGetLastActivity(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}
	page.lastActivity = response.Data;
	page.currentDebt = page.lastActivity.CurrentDebt;
	let currentDeptValue = document.getElementById("current-dept-value");
	currentDeptValue.innerHTML = page.lastActivity.CurrentDebt+" $";
	openPurchaseModal();
	openPaymentModal();
	openReturnModal();
	
}
function handleGetActivities(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}
	page.activities = response.Data;
	for (i = 0; i < page.activities.length; i++) {
		let activity = page.activities[i];
		if (activity.ActivityType == 0 && activity.Amount == 0) {
			continue;
		}
		else {
			
			appendActivity(activity, activity.ActivityType);
        }

	}

}
/*function tryInsertPurchase() {
	let description = document.getElementById("purchase-create-description").value;
	let productAmount = parseInt(document.getElementById("purchase-create-productamount").value);
	let paymentAmount = document.getElementById("purchase-create-paymentamount").value;
	if (paymentAmount == "" || paymentAmount == NaN || paymentAmount == undefined || paymentAmount == " ") {

		paymentAmount = 0;
	}

	let userId = page.onlineUser.Id;
	page.currentDebt = page.currentDebt + (productAmount - parseInt(paymentAmount));
	let newDebtValue = page.currentDebt;
	if (description == "" ||
		description == " ") {
		let message = "Description is required !";
		return showError(message);

	}
	if (productAmount == "") {
		return showError("Product amount is required !");
	}
	else if (!parseInt(productAmount))
	{
		
		return showError("Product amount can not be letter");
	}

	let customerId = page.customerId;

	let data = {
		Description: description,
		CustomerId: customerId,
		Amount: productAmount,
		ActivityType: 0,
		UserId: userId,
		CurrentDebt: newDebtValue,

	};
	
	httpRequest("api/Customer/CreateActivity/?paymentAmount=" + paymentAmount, "POST", data, handleInsertActivity, showError.bind(null, "System Error"));
}*/
function tryInsertPurchase() {
	let description = document.getElementById("purchase-create-description").value;
	let productAmountString = document.getElementById("purchase-create-productamount").value.toString().replace(",", ".");
	let productAmount = parseFloat(productAmountString);
	let PaymentAmountString = document.getElementById("purchase-create-paymentamount").value.toString().replace(",", ".");
	if (PaymentAmountString == "" || PaymentAmountString == NaN || PaymentAmountString == undefined || PaymentAmountString == " ") {

		PaymentAmountString = "0";
	}
	let paymentAmount = parseFloat(PaymentAmountString);
	let userId = page.onlineUser.Id;
	
	
	if (description == "" ||
		description == " ") {
		let message = "Description is required !";
		return showError(message);

	}
	if (productAmountString == "") {
		return showError("Product amount is required !");
	}
	else if (!parseInt(productAmountString)) {

		return showError("Product amount can not be letter");
	}

	let customerId = page.customerId;

	let data = {
		Description: description,
		CustomerId: customerId,
		Amount: productAmount,
		ActivityType: 0,
		UserId: userId,
		

	};

	httpRequest("api/Customer/CreateActivity/?paymentAmount=" + paymentAmount, "POST", data, handleInsertActivity, showError.bind(null, "System Error"));
	var modal = document.getElementById("purchase-modal");
	modal.style.display = "none";
}
/*function tryInsertPayment() {
	let description = document.getElementById("payment-create-description").value;
	let paymentAmount = parseInt(document.getElementById("payment-create-paymentamount").value);
	let userId = page.onlineUser.Id;
	page.currentDebt = page.currentDebt - paymentAmount;
	let newDebtValue = page.currentDebt;

	if (description == "") {
		let message = "Description is required !";
		return showError(message);

	}
	if (paymentAmount == "")
	{
		return showError("Payment amount is required !");
    }
	else if (!parseInt(paymentAmount)) {
		return showError("Payment amount can not be letter");
	}

	let customerId = page.customerId;

	let data = {
		Description: description,
		CustomerId: customerId,
		Amount: paymentAmount,
		ActivityType: 1,
		UserId: userId,
		CurrentDebt: newDebtValue,

	};

	httpRequest("api/Customer/CreateActivity/?paymentAmount=" + paymentAmount, "POST", data, handleInsertActivity, showError.bind(null, "System Error"));
}*/
function tryInsertPayment()
{
	let description = document.getElementById("payment-create-description").value;
	let paymentAmountString = document.getElementById("payment-create-paymentamount").value.toString().replace(",", ".");
	let paymentAmount = parseFloat(paymentAmountString);
	let userId = page.onlineUser.Id;
	if (paymentAmount > page.currentDebt) {

		let msgDiv = document.getElementById("message-div");
		msgDiv.style.display = "block";
		let messageBox = document.getElementById("message-box");
		messageBox.innerHTML = paymentAmount - page.currentDebt + " TL PARA ÜSTÜ ÖDEYİNİZ!";
	}

	if (description == "") {
		let message = "Description is required !";
		return showError(message);

	}
	if (paymentAmount == "") {
		return showError("Payment amount is required !");
	}
	else if (!parseInt(paymentAmount)) {
		return showError("Payment amount can not be letter");
	}

	let customerId = page.customerId;

	let data = {
		Description: description,
		CustomerId: customerId,
		Amount: paymentAmount,
		ActivityType: 1,
		UserId: userId,

	};

	httpRequest("api/Customer/CreateActivity/?paymentAmount=" + paymentAmount, "POST", data, handleInsertActivity, showError.bind(null, "System Error"));
	var modal = document.getElementById("payment-modal");
	modal.style.display = "none";
}
function tryInsertReturn() {
	let description = document.getElementById("return-create-description").value;
	let paymentAmountString = document.getElementById("return-create-productamount").value.toString().replace(",", ".");
	let paymentAmount = parseFloat(paymentAmountString);
	let userId = page.onlineUser.Id;
	if (paymentAmount > page.currentDebt) {

		let msgDiv = document.getElementById("message-div");
		msgDiv.style.display = "block";
		let messageBox = document.getElementById("message-box");
		messageBox.innerHTML = paymentAmount - (paymentAmount- page.currentDebt) + " TL PARA IADESI YAPINIZ!";
	}
	


	if (description == "") {
		let message = "Description is required !";
		return showError(message);

	}
	if (paymentAmount == "") {
		return showError("Payment amount is required !");
	}
	else if (!parseInt(paymentAmount)) {
		return showError("Payment amount can not be letter");
	}

	let customerId = page.customerId;

	let data = {
		Description: description,
		CustomerId: customerId,
		Amount: paymentAmount,
		ActivityType: 2,
		UserId: userId,


	};

	httpRequest("api/Customer/CreateActivity/?paymentAmount=" + paymentAmount, "POST", data, handleInsertActivity, showError.bind(null, "System Error"));
	var modal = document.getElementById("return-modal");
	modal.style.display = "none";
}
function handleInsertActivity(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	page.currentActivities = response.Data;
	for (let i = 0; i < page.currentActivities.length; i++) {
		let activity = page.currentActivities[i];
		let activityType = activity.ActivityType;
		appendActivity(activity, activityType);		
    }
	tryGetLastActivityByCustomerId(page.customerId);

}
function redirectToCustomerProfile(customerId) {
	redirect("User/CustomerProfile/" + customerId);
}
function appendActivity(activity, activityType) {
	let activityClass;
	let activityName;
	let amountType;
	if (activityType == 0) {
		activityClass = "purchase-activity";
		activityName = "Purchase";
		amountType = "Product";
	}
	if (activityType == 1) {
		activityClass = "payment-activity";
		activityName = "Payment";
		amountType = "Payment";
	}
	if (activityType == 2) {
		activityClass = "return-activity";
		activityName = "Return";
		amountType = "Returned Product";
	}
	if (activityType == 3) {
		activityClass = "refund-activity";
		activityName = "Refund";
		amountType = "Refund";
	}

	let activityTemplate = '<div class="clearfix activity" id="activity-id-##activity.Id##">';
	activityTemplate += '<div class="activity-info ##activityClass##">' + activityName + '</div>';
	activityTemplate += '<div class="activity-info ##activityClass##">Description: ##activity.Description##</div>';
	activityTemplate += '<div class="activity-info ##activityClass##">' + amountType + ' Amount: ##activity.Amount## $</div>';
	activityTemplate += '</div>';

	let activityHtmlString = activityTemplate
		.split("##activity.Id##").join(activity.Id)//.replace("##user.Id##", userModel.Id)
		.split("##activity.Description##").join(activity.Description)//.replace("##user.Username##", userModel.Username)
		.split("##activity.Amount##").join(activity.Amount)//.replace("##user.Email##", userModel.Email)
		.split("##activityClass##").join(activityClass)//.replace("##user.Email##", userModel.Email)

	let activityHtml = toDom(activityHtmlString);

	let activityListDiv = document.getElementById("activity-list");
	//activityListDiv.appendChild(activityHtml);
	activityListDiv.insertBefore(activityHtml, activityListDiv.childNodes[0]);

}
function openPurchaseModal() {



	var modal = document.getElementById("purchase-modal");
	var btn = document.getElementById("purchase-btn");
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
function openPaymentModal() {



	var modal = document.getElementById("payment-modal");
	var btn = document.getElementById("payment-btn");
	var span = document.getElementsByClassName("close")[1];
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
function openReturnModal() {



	var modal = document.getElementById("return-modal");
	var btn = document.getElementById("return-btn");
	var span = document.getElementsByClassName("close")[2];
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
function showError(message) {
	let errorDiv = document.getElementById("error");
	errorDiv.innerHTML = message;
	errorDiv.style.display = "block";
}

function hideError() {
	let errorDiv = document.getElementById("error");
	errorDiv.style.display = "none";
}
