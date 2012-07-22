using System;
using System.Linq;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Security;
using PhotoCache.Core.Models;
using PhotoCache.Core.Services;

namespace PhotoCache.Web.Authentication
{
    public class UserDatabase : IUserMapper
    {
        private static IModelService<UserModel> _users;

        public UserDatabase(IModelService<UserModel> users)
        {
            _users = users;
        }

        public IUserIdentity GetUserFromIdentifier(Guid id, NancyContext context)
        {
            var user = _users.Load(id);
            return new UserIdentity { User = user, UserName = user.UserName };
        }

        public static UserModel ValidateUser(string username, string password)
        {
            return _users.Query().FirstOrDefault(x => x.StoredUserName == username.ToLower() &&
                                                      x.Password == password.ToLower());
        }
    }
}