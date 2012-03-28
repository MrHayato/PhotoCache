using MongoDB.Driver;
using PhotoCache.Core.Models;

namespace PhotoCache.Web.Modules.ViewModules
{
    public class HelloViewModule : BaseModule
    {
        private readonly MongoCollection<TestModel> _collection;

        public HelloViewModule()
        {
            _collection = Database.GetCollection<TestModel>("TestModel");
            Get["/"] = x => View["index"];
            Get["/test"] = x => View["test/index", _collection.FindAll()];
        }
    }
}
