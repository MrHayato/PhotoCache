using System;
using System.Linq;
using MongoDB.Driver.Builders;
using Nancy.Authentication.Forms;
using Nancy.Security;
using PhotoCache.Core.Models;
using PhotoCache.Core.Persistence;

namespace PhotoCache.Web.Authentication
{
    public class UserDatabase : IUserMapper
    {
        private static IMongoRepository<UserModel> _users; 

        public UserDatabase(IMongoRepository<UserModel> users)
        {
            _users = users;
        }

        public IUserIdentity GetUserFromIdentifier(Guid id)
        {
            var user = _users.Get(id);
            return new UserIdentity { User = user, UserName = user.UserName };
        }

        public static UserModel ValidateUser(string username, string password)
        {
            return _users.CreateQuery(
                Query.And(
                    Query.EQ("StoredUserName", username.ToLower()), 
                    Query.EQ("Password", password))).FirstOrDefault();
        }
    }
}