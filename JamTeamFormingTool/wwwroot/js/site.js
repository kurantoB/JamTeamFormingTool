// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

window.getRoleCategoryRoles = (selectId) => {
	let sel = window.document.getElementById(selectId);
	var results = [];
	var i;
	for (i = 0; i < sel.options.length; i++) {
		if (sel.options[i].selected) {
			results[results.length] = sel.options[i].value;
		}
	}
	return results;
}