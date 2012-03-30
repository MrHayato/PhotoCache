namespace PhotoCache.Web.Modules.ViewModules
{
    public class HelloViewModule : BaseModule
    {
        public HelloViewModule()
        {
            Get["/"] = x => View["Index.cshtml"];
        }
    }
}
