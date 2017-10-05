(function ($) {
	// Bootstrap: Ensures that upon failing validation the error styles are applied.
	$.validator.setDefaults({
		highlight: function (element) {
			$(element).closest(".form-group").removeClass("has-success");
			$("[data-valmsg-for = " + element.name + "]").closest(".form-group").removeClass("has-success");
			$(element).closest(".form-group").addClass("has-error");
			$("[data-valmsg-for = " + element.name + "]").closest(".form-group").addClass("has-error");
		},
		unhighlight: function (element) {
			$(element).closest(".form-group").removeClass("has-error");
			$("[data-valmsg-for = " + element.name + "]").closest(".form-group").removeClass("has-error");
			$(element).closest(".form-group").addClass("has-success");
			$("[data-valmsg-for = " + element.name + "]").closest(".form-group").addClass("has-success");
		},
		ignore: ['']
	});
});