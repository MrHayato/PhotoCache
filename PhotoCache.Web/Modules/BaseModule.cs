using System.Configuration;
using MongoDB.Driver;
using Nancy;

namespace PhotoCache.Web.Modules
{
    public class BaseModule : NancyModule
    {
        public MongoDatabase Database
        {
            get
            {
                return MongoDatabase.Create(GetMongoDbConnectionString());
            }
        }

        private string GetMongoDbConnectionString()
        {
            return ConfigurationManager.AppSettings.Get("MONGOHQ_URL") ??
                "mongodb://localhost/Things";
        }
    }
}