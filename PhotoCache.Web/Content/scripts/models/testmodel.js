var TestModel = Backbone.Model.extend({
});

var TestCollection = Backbone.Collection.extend({
    model: TestModel,
    url: "/api/tests"
});