using System;
using Nancy;
using Nancy.Authentication.Forms;
using PhotoCache.Core.Models;
using PhotoCache.Core.Persistence;
using PhotoCache.Web.Authentication;
using PhotoCache.Web.Helpers;

namespace PhotoCache.Web.Modules.APIModules
{
    public class LoginModule : BaseAPIModule
    {
        private const int expiryDays = 1; //Number of days a user stays logged in without the "remember me" option
        private IMongoRepository<UserModel> _users;

        public LoginModule(IMongoRepository<UserModel> users)
        {
            _users = users;
            Post["/login"] = x => Login();
            Get["/logout"] = x => Logout();
        }

        private Response Login()
        {
            var username = Request.Form.Username;
            var password = Request.Form.Password;
            var user = UserDatabase.ValidateUser(username, password);

            if (user == null)
                return Response.Error("UserName or password is incorrect.");

            user.LastLogin = DateTime.Now;
            _users.Update(user);

            DateTime? expiry = null;
            if (!Request.Form.RememberMe.HasValue)
                expiry = DateTime.Now.AddDays(expiryDays);

            return this.LoginAndRedirect((Guid)user.Id, expiry);
        }

        private Response Logout()
        {
            return this.LogoutAndRedirect("~/");
        }
    }
}