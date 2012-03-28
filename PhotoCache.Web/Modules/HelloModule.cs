using System;
using MongoDB.Driver;
using Nancy;
using PhotoCache.Web.Helpers;

namespace PhotoCache.Web.Modules
{
    public class TestModel
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
    }

    public class HelloModule : BaseModule
    {
        private readonly MongoCollection<TestModel> _collection;

        public HelloModule()
        {
            _collection = Database.GetCollection<TestModel>("TestModel");
            Get["/"] = x => "Hello World";
            Get["/test"] = x => GetTestData();
            Get["/test/{value}"] = x => CreateTestData(x.value);
        }

        private Response CreateTestData(string value)
        {
            var newModel = new TestModel { Id = new Guid(), Value = value };
            _collection.Insert(newModel);
            return Response.AsJson(newModel).Created();
        }

        private Response GetTestData()
        {
            return Response.AsJson(_collection.FindAll());
        }
    }
}
