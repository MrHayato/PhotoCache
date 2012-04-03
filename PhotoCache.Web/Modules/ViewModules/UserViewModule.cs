using Nancy.Authentication.Forms;

namespace PhotoCache.Web.Modules.ViewModules
{
    public class UserViewModule : BaseModule
    {
        public UserViewModule()
        {
            Get["/register"] = x => RenderView("User/Register.cshtml");
            Get["/login"] = x => RenderView("User/Login.cshtml");
            Get["/logout"] = x => this.LogoutAndRedirect("~/");
        }
    }
}