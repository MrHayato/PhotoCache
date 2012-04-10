using System.Collections.Generic;
using Nancy.Security;
using PhotoCache.Core.Models;

namespace PhotoCache.Web.Authentication
{
    public class UserIdentity : IUserIdentity
    {
        public UserModel User { get; set; }
        public string UserName { get { return User.UserName; } set { User.UserName = value; } }
        public IEnumerable<string> Claims { get; set; }

        public UserIdentity()
        {
            Claims = new List<string>();
        }
    }
}