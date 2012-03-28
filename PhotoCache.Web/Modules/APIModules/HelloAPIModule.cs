using System;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Nancy;
using PhotoCache.Core.Models;
using PhotoCache.Web.Helpers;

namespace PhotoCache.Web.Modules.APIModules
{
    public class HelloAPIModule : BaseAPIModule
    {
        private readonly MongoCollection<TestModel> _collection;

        public HelloAPIModule()
        {
            _collection = Database.GetCollection<TestModel>("TestModel");
            Get["/tests"] = x => GetTests();
            Get["/test/{Id}"] = x => GetTest((Guid)x.Id);
        }

        private Response GetTest(Guid id)
        {
            return Response.AsJson(_collection.Find(Query.EQ("_id", id)));
        }

        private Response GetTests()
        {
            return Response.AsJson(_collection.FindAll()).Ok();
        }
    }
}