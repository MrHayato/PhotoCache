using Nancy;
using PhotoCache.Core.Models;
using PhotoCache.Core.Persistence;
using PhotoCache.Core.ReadModels;
using PhotoCache.Core.Services;
using PhotoCache.Web.Helpers;

namespace PhotoCache.Web.Modules.APIModules.Secure
{
    public class SecureUserModule : SecureAPIModule
    {
        private IModelService<UserModel> _users;

        public SecureUserModule(IRavenRepository<SessionModel> sessions, IModelService<UserModel> users) : base(sessions)
        {
            _users = users;
            Get["/users"] = x => GetUsers();
        }

        private Response GetUsers()
        {
            return Response.AsJson(_users.LoadAll()).Ok();
        }
    }
}