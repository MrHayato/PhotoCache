using System;
using System.Linq;
using Nancy;
using Nancy.Authentication.Forms;
using PhotoCache.Core.Extensions;
using PhotoCache.Core.Logging;
using PhotoCache.Core.Models;
using PhotoCache.Core.Persistence;
using PhotoCache.Core.ReadModels;
using PhotoCache.Core.Services;
using PhotoCache.Web.Helpers;
using Raven.Client.Linq;

namespace PhotoCache.Web.Modules.APIModules
{
    public class LoginModule : BaseAPIModule
    {
        private readonly ILogger _logger;
        private IModelService<UserModel> _users;
        private readonly IRavenRepository<SessionModel> _sessions;
        private const int MaxLoginRetries = 5; //Number of login attempts

        public LoginModule(ILogger logger, IModelService<UserModel> users, IRavenRepository<SessionModel> sessions)
        {
            _logger = logger;
            _users = users;
            _sessions = sessions;
            Post["/login"] = x => Login();
            Get["/logout"] = x => Logout();
        }

        private Response Login()
        {
            var username = (string)Data.UserName;
            var password = (string)Data.Password;
            var users = _users.Query().Where(u => u.StoredUserName == username.ToLower()).ToList();
            Response response;

            if (users.Count == 0)
            {
                response = Response.Error(Res.User.Login.IncorrectLogin);
            }
            else if (users.Count > 1) //More than one user with the same name?!
            {
                //Log this weird occurance and deny
                _logger.Log(new LogMessageModel("More than one user found with the username '" + username + "'."));
                response = Response.Error(Res.User.Login.IncorrectLogin);
            }
            else
            {
                var user = users.First();
                var hashedPW = (password + user.Id).GetSha1Hash(); //Using user's id as the salt

                if (user.AccountLocked)
                {
                    //Too many attempts.
                    user.LastLogin = DateTime.Now;
                    _logger.Log(new LogMessageModel("Too many login attempts for username '" + username + "' under the IP address: " + Request.UserHostAddress));
                    response = Response.Error(Res.User.Login.TooManyAttempts);
                }
                else if (user.Password != hashedPW)
                {
                    var tries = user.LoginAttempts + 1;

                    user.LastLogin = DateTime.Now;
                    user.LoginAttempts = tries;

                    if (tries >= MaxLoginRetries)
                    {
                        user.AccountLocked = true;
                        _logger.Log(new LogMessageModel("Too many login attempts for username '" + username + "' under the IP address: " + Request.UserHostAddress));
                    }

                    response = Response.Error(Res.User.Login.IncorrectLogin);
                }
                else
                {
                    //Success!
                    var session = new SessionModel(!Request.Form.RememberMe.HasValue);

                    _sessions.Store(session);

                    if (Context.Items.ContainsKey(SecureAPIModule.SessionItemKey))
                        Context.Items[SecureAPIModule.SessionItemKey] = session;
                    else
                        Context.Items.Add(SecureAPIModule.SessionItemKey, session);
                    
                    user.LastLogin = DateTime.Now;
                    user.LoginAttempts = 0;

                    response = Response.AsRedirect("~/");
                    response.StatusCode = HttpStatusCode.OK;
                }

                _users.Update(user);
            }

            return response;
        }

        private Response Logout()
        {
            return this.LogoutAndRedirect("~/");
        }
    }
}