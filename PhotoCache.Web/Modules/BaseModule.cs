using Nancy;
using PhotoCache.Core.Models;
using PhotoCache.Web.Authentication;

namespace PhotoCache.Web.Modules
{
    public class BaseModule : NancyModule
    {
        public BaseModule(string modulePath) : base(modulePath)
        {
        }

        public BaseModule()
        {
        }

        public UserModel GetUserModel()
        {
            var userIdentity = ((UserIdentity)Context.CurrentUser);
            UserModel model = null;

            if (userIdentity != null)
                model = userIdentity.User;

            return model;
        }

        public Response RenderView(string view)
        {
            return View[view, new { CurrentUser = GetUserModel()}];
        }

        public Response RenderView(string view, object model)
        {
            return View[view, new { Model = model, CurrentUser = GetUserModel() }];
        }
    }
}