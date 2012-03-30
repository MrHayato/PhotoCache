using System.IO;
using Cassette.Configuration;
using Cassette.HtmlTemplates;
using Cassette.Scripts;
using Cassette.Stylesheets;

namespace PhotoCache.Web
{
    /// <summary>
    /// Configures the Cassette asset modules for the web application.
    /// </summary>
    public class CassetteConfiguration : ICassetteConfiguration
    {
        private const string ScriptsFolder = "Content/scripts";
        private const string StylesheetsFolder = "Content/css";
        private const string TemplatesFolder = "Content/templates";

        public void Configure(BundleCollection bundles, CassetteSettings settings)
        {
            //These are the libraries with CDN. Set the path to the local asset
            //  for debugging
            bundles.AddUrlWithLocalAssets(
                "//ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js",
                new LocalAssetSettings
                    {
                        Path = ScriptsFolder + "/libs/jquery-1.7.2.js",
                        FallbackCondition = "!window.jQuery"
                    }
                );

            //Set up libraries. The libraries listed here do not have a CDN, so we'll let
            //  Cassette minify these for us.
            bundles.Add<ScriptBundle>(ScriptsFolder + "/libs", new[]
                {
                    "bootstrap-2.0.2.js",
                    "underscore-1.3.1.js",
                    "backbone-0.9.2.js",
                    "backbone.modelbinding-0.5.0.js"
                });

            //Our scripts
            bundles.AddPerIndividualFile<ScriptBundle>(ScriptsFolder + "/models"); //backbone models
            bundles.AddPerIndividualFile<ScriptBundle>(ScriptsFolder + "/views"); //backbone views
            
            bundles.Add<StylesheetBundle>(StylesheetsFolder + "/libs");
            bundles.AddPerIndividualFile<StylesheetBundle>(StylesheetsFolder, new FileSearch
            {
                SearchOption = SearchOption.TopDirectoryOnly
            });
        }
    }
}