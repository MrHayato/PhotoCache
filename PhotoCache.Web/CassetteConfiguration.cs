using System.IO;
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
        private const string ScriptsFolder = "Content/scripts";
        private const string StylesheetsFolder = "Content/css";

        public void Configure(BundleCollection bundles, CassetteSettings settings)
        {
            //Set up third-party libraries to automatically minify and bundle
            bundles.Add<ScriptBundle>(ScriptsFolder + "/libs");

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