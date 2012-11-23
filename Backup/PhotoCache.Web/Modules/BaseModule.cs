

using Nancy;
using PhotoCache.Core.ReadModels;
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
//
//        public ViewRenderer RenderView(string view)
//        {
//            return View[view, new ViewModel { CurrentUser = GetUserModel() }];
//        }
//
//        public ViewRenderer RenderView(string view, object model)
//        {
//            return View[view, new ViewModel { Model = model, CurrentUser = GetUserModel() }];
//        }
//        
    }
}