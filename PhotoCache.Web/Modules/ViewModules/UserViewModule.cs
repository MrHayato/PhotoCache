using Nancy.Authentication.Forms;
using PhotoCache.Web.Models;

namespace PhotoCache.Web.Modules.ViewModules
{
    public class UserViewModule : BaseModule
    {
        public UserViewModule()
        {
            Get["/register"] = x => View["User/Register.cshtml", new ViewModel { CurrentUser = GetUserModel() }];
            Get["/login"] = x => View["User/Login.cshtml", new ViewModel { CurrentUser = GetUserModel() }];
            Get["/logout"] = x => this.LogoutAndRedirect("~/");
        }
    }
}