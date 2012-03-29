using Cassette.Configuration;
using Cassette.Scripts;
using Cassette.Stylesheets;

namespace PhotoCache.Web
{
    /// <summary>
    /// Configures the Cassette asset modules for the web application.
    /// </summary>
    public class CassetteConfiguration : ICassetteConfiguration
    {
        public void Configure(BundleCollection bundles, CassetteSettings settings)
        {
            //Set up libraries. The libraries listed here do not have a CDN, so we'll let
            //  Cassette minify these for us.
            bundles.Add<ScriptBundle>("Content/scripts/libs/", new[]
                {
                    "underscore-1.3.1.js",
                    "backbone-0.9.2.js"
                });

            //These are the libraries with CDN. Set the path to the local asset
            //  for debugging
            bundles.AddUrlWithLocalAssets(
                "//ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js",
                new LocalAssetSettings
                    {
                        Path = "Content/scripts/libs/jquery-1.7.2.js",
                        FallbackCondition = "!window.jQuery"
                    }
                );

            bundles.Add<ScriptBundle>("Content/scripts/models/");
            bundles.Add<ScriptBundle>("Content/scripts/views/");

        }
    }
}