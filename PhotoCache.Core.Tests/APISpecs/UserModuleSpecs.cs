using System.Linq;
using Machine.Specifications;
using Nancy;
using Nancy.Testing;
using PhotoCache.Core.Extensions;
using PhotoCache.Core.ReadModels;

// ReSharper disable UnusedMember.Local
namespace PhotoCache.Core.Specs.APISpecs
{
    public class UserModuleSpecs : BaseModuleSpecs<UserModel>
    {
        protected class Endpoints
        {
            public static string Validate = "/user/validate";
            public static string Register = "/user/register";
        }

        protected static int _userCount;
        protected static dynamic _lastEstablishedUser;

        protected static dynamic CreateValidUser()
        {
            _userCount++;
            return new {
                           UserName = "TestUser" + _userCount,
                           StoredUserName = "testuser" + _userCount,
                           Password = "TestPassword",
                           Email = "someuser@somewebsite.com",
                           FirstName = "Test",
                           LastName = "User " + _userCount
                       };
        }

        protected static dynamic CreateInvalidUser()
        {
            return new {
                           UserName = "U",
                           Password = "P",
                           Email = "E",
                           FirstName = "F",
                           LastName = "L"
                       };
        }

        Establish that = () =>
                         {
                             _lastEstablishedUser = CreateValidUser();
                             Repository.Store(new UserModel
                                                  {
                                                      UserName = _lastEstablishedUser.UserName,
                                                      StoredUserName = ((string)_lastEstablishedUser.UserName).ToLower(),
                                                      Password = _lastEstablishedUser.Password,
                                                      Email = _lastEstablishedUser.Email,
                                                      FirstName = _lastEstablishedUser.FirstName,
                                                      LastName = _lastEstablishedUser.LastName
                                                  });
                         };

    }

    public class when_creating_a_new_valid_user : UserModuleSpecs
    {
        private static dynamic _user = CreateValidUser();
        private static string _username = _user.StoredUserName;
        private static string _password = _user.Password;

        private Because of = () => JsonRequest(Endpoints.Register, _user, POST);
        private It should_return_201_created = () => Response.StatusCode.ShouldEqual(HttpStatusCode.Created);
        private It should_have_the_created_user_in_repo = () => Repository.Query()
            .Any(q => q.StoredUserName == _username)
            .ShouldEqual(true);
        private It should_have_a_hashed_password_in_the_response = () => Response.Body["Password"]
            .ShouldContain(_password.GetSha1Hash());
    }

    public class when_creating_a_user_that_already_exists : UserModuleSpecs
    {
        private Because of = () => JsonRequest(Endpoints.Register, _lastEstablishedUser, POST);

        private It should_return_400_bad_request = () => Response.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);
        private It should_return_user_exists_message = () => 
            Response.Body.AsString().ShouldContain(Res.UserModel.UsernameExists);
    }

    public class when_creating_a_new_invalid_user : UserModuleSpecs
    {
        private Because of = () => JsonRequest(Endpoints.Register, CreateInvalidUser(), POST);
        private It should_return_400_bad_request = () => Response.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);
    }

    public class when_validating_a_valid_username : UserModuleSpecs
    {
        private Establish that = () => Query.Add("UserName", "SomeValidUserName");
        private Because of = () => Request(Endpoints.Validate);
        private It should_return_200_ok = () => Response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    public class when_validating_a_valid_password : UserModuleSpecs
    {
        private Establish that = () => Query.Add("Password", "SomeValidUserPassword");
        private Because of = () => Request(Endpoints.Validate);
        private It should_return_200_ok = () => Response.StatusCode.ShouldEqual(HttpStatusCode.OK);   
    }

    public class when_validating_an_invalid_username : UserModuleSpecs
    {
        private Establish that = () => Query.Add("UserName", "a");
        private Because of = () => Request(Endpoints.Validate);
        private It should_return_400_bad_request = () => Response.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);
    }

    public class when_validating_an_invalid_password : UserModuleSpecs
    {
        private Establish that = () => Query.Add("Password", "a");
        private Because of = () => Request(Endpoints.Validate);
        private It should_return_400_bad_request = () => Response.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);
    }
}
