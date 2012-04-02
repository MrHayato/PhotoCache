using Nancy;

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
    }
}