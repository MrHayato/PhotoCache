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
            Post["/newuser"] = x => CreateNewUser();
        }

        private Response CreateNewUser()
        {
            if (!Data.UserName.HasValue || !Data.Password.HasValue)
                return Response.Error("No username or password specified.");

            var username = (string)Data.UserName;
            var password = (string)Data.Password;
            
            if (_users.CreateQuery(Query.EQ("StoredUsername", username)).Any())
                return Response.Error("The username '" + username + "' already exists.");

            var user = new UserModel
                {
                    FirstName = DateTime.Now.ToShortTimeString(),
                    LastName = (string)Data.FirstName,
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