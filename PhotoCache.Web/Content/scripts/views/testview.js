/// <reference path="../models/TestModel.js" />

var TestView = Backbone.View.extend({
    el: $("content"),
    template: _.template($("#test-model-template").html()),
    initialize: function () {
        var self = this;

        this.model = new TestCollection();
        this.model.fetch().success(function () {
            self.render();
        });
    },
    render: function () {
        var $table = $("#test-models").empty();
        var self = this;
        
        this.model.each(function (model) {
            $table.append(self.template(model.toJSON()));
        });
    }
});

var view = new TestView();