using Nancy;
using Nancy.Security;
using PhotoCache.Core.Models;
using PhotoCache.Core.Services;
using PhotoCache.Web.Helpers;

namespace PhotoCache.Web.Modules.APIModules
{
    public class SecureUserModule : BaseAPIModule
    {
        private IModelService<UserModel> _users;

        public SecureUserModule(IModelService<UserModel> users)
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