App.Models.Registration = Backbone.Model.extend({
    url: "/api/user/register",
    validateUrl: "/api/user/validate",
    initialize: function () {
    },
    validate: function (attrs) {
        var queryString = "";
        var first = true;
        var self = this;

        for (var attr in attrs) {
            queryString +=
                (first ? "?" : "&") +
                attr + "=" + attrs[attr];

            first = false;
        }

        var onError = function (err) {
            self.trigger("error", self, err);
        };

        var onSuccess = function () {
            self.trigger("validationPassed");
        };

        $.when($.ajax(this.validateUrl + queryString)
            .success(onSuccess)
            .error(onError));
    }
});