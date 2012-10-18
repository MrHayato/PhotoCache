using System;
using Machine.Specifications;
using Nancy;
using Nancy.Testing;
using PhotoCache.Core.Extensions;
using PhotoCache.Core.ReadModels;
using PhotoCache.Web.Modules.APIModules;

namespace PhotoCache.Core.Specs.APISpecs
{
    public class BaseLoginSpecs : BaseModuleSpecs<UserModel>
    {
        protected class Endpoints
        {
            public static string Login = "/login";
            public static string Logout = "/logout";
        }

        protected static bool _userExists;
        protected static Guid _userId;

        Establish that = () =>
        {
            if (!_userExists)
            {
                _userId = Guid.NewGuid();

                Repository.Store(new UserModel
                {
                    Id = _userId,
                    UserName = "TestLoginUser",
                    StoredUserName = "testloginuser",
                    Password = ("Password" + _userId).GetSha1Hash(),
                    Email = "mrhayato@gmail.com",
                    FirstName = "Test",
                    LastName = "Login User",
                    LastLogin = DateTime.MinValue
                });

                _userExists = true;
            }
            else
            {
                var user = Repository.Load(_userId);

                user.AccountLocked = false;
                user.LoginAttempts = 0;
                user.LastLogin = DateTime.MinValue;

                Repository.Store(user);
            }
        };
    }

    public class when_attempting_to_log_in_as_a_valid_user : BaseLoginSpecs
    {
        private Because of = () =>
            JsonRequest(Endpoints.Login, new { UserName = "TestLoginUser", Password = "Password" }, POST);

        private It should_return_200_ok = () => Response.StatusCode.ShouldEqual(HttpStatusCode.OK);
        private It should_set_the_last_login_date = () => Repository.Load(_userId).LastLogin.ShouldBeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(1000));
        private It should_set_the_session_object = () => Response.Context.Items.ContainsKey(SecureAPIModule.SessionItemKey).ShouldBeTrue();
    }

    public class when_logging_in_as_an_invalid_user : BaseLoginSpecs
    {
        private Because of = () =>
            JsonRequest(Endpoints.Login, new { UserName = "TestLoginUser", Password = "Passwords" }, POST);

        private It should_increase_the_login_attempts = () => Repository.Load(_userId).LoginAttempts.ShouldBeGreaterThan(0);
        private It should_return_an_error = () => Response.Body.AsString().ShouldContain(Res.User.Login.IncorrectLogin);
        private It should_return_a_bad_request = () => Response.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);
    }

    public class when_logging_in_as_an_invalid_user_multiple_times : BaseLoginSpecs
    {
        private Establish context = () =>
        {
            for (int i = 0; i < 5; i++)
                JsonRequest(Endpoints.Login, new { UserName = "TestLoginUser", Password = "Passwords" }, POST);
        };

        private Because of = () =>
            JsonRequest(Endpoints.Login, new { UserName = "TestLoginUser", Password = "Passwords" }, POST);

        private It should_have_many_login_attempts = () => Repository.Load(_userId).LoginAttempts.ShouldBeGreaterThanOrEqualTo(5);
        private It should_be_a_locked_account = () => Repository.Load(_userId).AccountLocked.ShouldBeTrue();
        private It should_return_a_lockout_error = () => Response.Body.AsString().ShouldContain(Res.User.Login.TooManyAttempts);
        private It should_return_a_bad_request = () => Response.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);
    }
}
