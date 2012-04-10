using Nancy;
using Nancy.Security;
using PhotoCache.Core.Models;
using PhotoCache.Core.Persistence;
using PhotoCache.Web.Helpers;

namespace PhotoCache.Web.Modules.APIModules
{
    public class SecureUserModule : BaseAPIModule
    {
        private IRavenRepository<UserModel> _users;

        public SecureUserModule(IRavenRepository<UserModel> users)
        {
            this.RequiresAuthentication();
            _users = users;
            Get["/users"] = x => GetUsers();
        }

        private Response GetUsers()
        {
            return Response.AsJson(_users.LoadAll()).Ok();
        }
    }
}