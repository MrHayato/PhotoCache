using System;
using FluentValidation;
using Nancy;
using Nancy.Authentication.Forms;
using PhotoCache.Core.Models;
using PhotoCache.Core.Services;
using PhotoCache.Web.Authentication;
using PhotoCache.Web.Helpers;

namespace PhotoCache.Web.Modules.APIModules
{
    public class LoginModule : BaseAPIModule
    {
        private IModelService<UserModel> _users;
        private const int expiryDays = 1; //Number of days a user stays logged in without the "remember me" option

        public LoginModule(IModelService<UserModel> users)
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
                return Response.Error(Res.User.Login.IncorrectLogin);

            try
            {
                user.LastLogin = DateTime.Now;
                _users.Update(user);
            }
            catch (ValidationException ex)
            {
                return Response.Error(ex.Errors);
            }

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