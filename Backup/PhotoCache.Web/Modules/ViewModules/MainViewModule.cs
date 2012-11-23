using Nancy;
using PhotoCache.Web.Models;

namespace PhotoCache.Web.Modules.ViewModules
{
    public class MainViewModule : NancyModule//: BaseModule
    {
        public MainViewModule()
        {
            Get["/"] = x => View["Index.cshtml"];//, new ViewModel { CurrentUser = GetUserModel() }];
            Get["/about"] = x => View["About.cshtml"];//, new ViewModel { CurrentUser = GetUserModel() }];
        }
    }
}