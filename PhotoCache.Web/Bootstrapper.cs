using System.Configuration;
using Cassette.Nancy;
using Nancy;
using Nancy.Conventions;

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

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            nancyConventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("css", @"Content\css")
            );

            nancyConventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("img", @"Content\img")
            );

            nancyConventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("scripts", @"Content\scripts")
            );
        }
    }
}