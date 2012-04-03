namespace PhotoCache.Web.Modules.ViewModules
{
    public class MainViewModule : BaseModule
    {
        public MainViewModule()
        {
            Get["/"] = x => RenderView("Index.cshtml");
            Get["/about"] = x => RenderView("About.cshtml");
        }
    }
}