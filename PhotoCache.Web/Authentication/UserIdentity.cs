using System.Collections.Generic;
using Nancy.Security;
using PhotoCache.Core.Models;

namespace PhotoCache.Web.Authentication
{
    public class UserIdentity : UserModel, IUserIdentity
    {
        public UserModel User { get; set; }
        public IEnumerable<string> Claims { get; set; }

        public UserIdentity()
        {
            Claims = new List<string>();
        }
    }
}