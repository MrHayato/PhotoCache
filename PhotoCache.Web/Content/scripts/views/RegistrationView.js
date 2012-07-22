App.Views.Registration = Backbone.View.extend({
    el: $("#registration-form"),
    initialize: function () {
        _.bindAll(this, "submit", "onError", "addError", "showSuccess", "saveSuccess", "removeMessages", "validationPassed");
        $(this.el).bind("submit", this.submit);

        this.model.bind("error", this.onError);
        this.model.bind("sync", this.saveSuccess);
        this.model.bind("validationPassed", this.validationPassed);
        Backbone.ModelBinding.bind(this);
    },
    submit: function () {
        var privacyChecked = $("#privacy-policy").attr("checked");

        this.removeMessages();

        if (!privacyChecked) {
            this.addError("privacy-policy", "You must accept the privacy policy");
        } else if (this.$("#Password").val() != this.$("#PasswordConfirm").val()) {
            this.addError("PasswordConfirm", "Your passwords do not match");
        } else {
            this.model.save();
        }

        return false;
    },
    onError: function (model, error) {
        var response = JSON.parse(error.responseText);
        var self = this;

        this.removeMessages();

        if (response && response.Messages) {
            $("input", this.el).each(function (i, inputField) {
                if (inputField.id in response.Messages)
                    self.addError(inputField.id, response.Messages[inputField.id]);
            });
        }
    },
    validationPassed: function () {
        this.removeMessages();
    },
    removeMessages: function () {
        var $controlGroup = this.$(".control-group").removeClass("success").removeClass("error");
        $controlGroup.find(".help-block").remove();
    },
    addError: function (fieldName, message) {
        var $errorControl = $("<p></p>").addClass("help-block").text(message);
        $("#" + fieldName).parents(".control-group").addClass("error").find(".controls").append($errorControl);
    },
    showSuccess: function (fieldName) {
        $("#" + fieldName).parents(".control-group").addClass("success");
    },
    saveSuccess: function () {
        window.location.replace("/");
    }
});
