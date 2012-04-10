using System;
using System.Linq;
using Nancy.Authentication.Forms;
using Nancy.Security;
using PhotoCache.Core.Models;
using PhotoCache.Core.Persistence;

namespace PhotoCache.Web.Authentication
{
    public class UserDatabase : IUserMapper
    {
        private static IRavenRepository<UserModel> _users; 

        public UserDatabase(IRavenRepository<UserModel> users)
        {
            _users = users;
        }

        public IUserIdentity GetUserFromIdentifier(Guid id)
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