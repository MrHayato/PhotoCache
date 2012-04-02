namespace PhotoCache.Web.Modules.ViewModules
{
    public class MainViewModule : BaseModule
    {
        public MainViewModule()
        {
            Get["/"] = x => View["Index.cshtml"];
        }
    }
}