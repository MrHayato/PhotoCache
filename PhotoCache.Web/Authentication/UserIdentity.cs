using System;
using System.Collections.Generic;
using Nancy.Security;
using PhotoCache.Core.ReadModels;

namespace PhotoCache.Web.Authentication
{
    public class UserIdentity : IUserIdentity
    {
        public string UserName
        {
            get { return User.UserName; }
            set { throw new InvalidOperationException("You may not modify a user's username"); }
        }
        public UserModel User { get; set; }
        public IEnumerable<string> Claims { get; set; }

        public UserIdentity()
        {
            Claims = new List<string>();
        }
    }
}