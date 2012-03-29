using System.Configuration;
using Cassette.Nancy;
using Nancy;

namespace PhotoCache.Web
{
    public class Bootstrapper : DefaultNancyBootstrapper 
    {
        public Bootstrapper()
        {
            bool optimize;
            bool.TryParse(ConfigurationManager.AppSettings.Get("OptimizeCassette"), out optimize);
            CassetteStartup.ShouldOptimizeOutput = optimize;
        }
    }
}