using Nancy;
using Nancy.Security;
using PhotoCache.Core.Models;
using PhotoCache.Core.Persistence;
using PhotoCache.Web.Helpers;

namespace PhotoCache.Web.Modules.APIModules
{
    public class UserSecureModule : BaseAPIModule
    {
        private IMongoRepository<UserModel> _users;

        public UserSecureModule(IMongoRepository<UserModel> users)
        {
            this.RequiresAuthentication();
            _users = users;
            Get["/users"] = x => GetUsers();
        }

        private Response GetUsers()
        {
            return Response.AsJson(_users.GetAll()).Ok();
        }
    }
}