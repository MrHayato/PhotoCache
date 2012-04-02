namespace PhotoCache.Web.Modules.ViewModules
{
    public class UserViewModule : BaseModule
    {
        public UserViewModule()
        {
            Get["/login"] = x => View["User/Login.cshtml"];
            Get["/register"] = x => View["User/Register.cshtml"];
        }
    }
}