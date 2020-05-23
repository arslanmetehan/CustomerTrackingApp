function toDom(htmlString) {
	let wrapper = document.createElement('div');
	wrapper.innerHTML = htmlString;
	return wrapper.firstChild;
}