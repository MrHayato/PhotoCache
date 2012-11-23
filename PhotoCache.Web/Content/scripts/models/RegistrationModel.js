App.Models.Registration = Backbone.Model.extend({
    url: "/api/user/register",
    validateUrl: "/api/user/validate",
    initialize: function () {
    },
    validate: function (attrs) {
        var queryString = "",
            first = true,
            self = this,
            attr,
            onError,
            onSuccess;

        onError = function (err) {
            self.trigger("error", self, err);
        };
        
        onSuccess = function () {
            self.trigger("validationPassed");
        };

        for (attr in attrs) {
            queryString +=
                (first ? "?" : "&") +
                attr + "=" + attrs[attr];

            first = false;
        }

        $.when($.ajax(this.validateUrl + queryString)
            .success(onSuccess)
            .error(onError));
    }
});