using System;
using System.Linq;
using MongoDB.Driver.Builders;
using Nancy;
using PhotoCache.Core.Models;
using PhotoCache.Core.Persistence;
using PhotoCache.Web.Helpers;

namespace PhotoCache.Web.Modules.APIModules
{
    public class UserModule : BaseAPIModule
    {
        private IMongoRepository<UserModel> _users;

        public UserModule(IMongoRepository<UserModel> users)
        {
            _users = users;
            Get["/newuser"] = x => CreateNewUser();
        }

        private Response CreateNewUser()
        {
            if (!Request.Query.username.HasValue || !Request.Query.password.HasValue)
                return Response.Error("No username or password specified.");

            var username = (string)Request.Query.username;
            var password = (string)Request.Query.password;
            
            if (_users.CreateQuery(Query.EQ("StoredUsername", username)).Any())
                return Response.Error("UserName already exists.");

            var user = new UserModel
                {
                    FirstName = DateTime.Now.ToShortTimeString(),
                    LastName = "TestUser",
                    UserName = username,
                    StoredUserName = username.ToLower(),
                    Password = password,
                    Role = Roles.User
                };

            var result = user.Validate();

            if (!result.IsValid)
                return Response.Error(result.Errors);

            _users.Create(user);
            return Response.AsJson(user).Created();
        }
    }
}