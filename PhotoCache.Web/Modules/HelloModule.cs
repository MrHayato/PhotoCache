using Nancy;

namespace PhotoCache.Web.Modules
{
    public class HelloModule : NancyModule
    {
        public HelloModule()
        {
            Get["/"] = x => "Hello World";
        }
    }
}
