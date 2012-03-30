using System;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Nancy;
using PhotoCache.Core.Models;
using PhotoCache.Web.Helpers;

namespace PhotoCache.Web.Modules.APIModules
{
    public class TestAPIModule : BaseAPIModule
    {
        private readonly MongoCollection<TestModel> _collection;

        public TestAPIModule()
        {
            _collection = Database.GetCollection<TestModel>("TestModel");
            Get["/tests"] = x => Response.AsJson(_collection.FindAll()).Ok();
            Get["/tests/{id}"] = x => Response.AsJson(_collection.Find(Query.EQ("_id", (Guid)x.id))).Ok();
        }
    }
}