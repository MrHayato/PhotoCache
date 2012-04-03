using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using Cassette.Nancy;
using MongoDB.Driver;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Conventions;
using Nancy.Session;
using PhotoCache.Core.Models;
using PhotoCache.Core.Persistence;
using PhotoCache.Web.Authentication;

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

        protected override void ApplicationStartup(TinyIoC.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            CookieBasedSessions.Enable(pipelines);
        }

        protected override void ConfigureApplicationContainer(TinyIoC.TinyIoCContainer container)
        {
            var connectionString = ConfigurationManager.AppSettings.Get("MONGOHQ_URL") ?? "mongodb://localhost:27017/photoCache";
            var dbName = connectionString.Split('/').Last();
            var server = MongoServer.Create(connectionString);
            var database = server.GetDatabase(dbName);

            container.Register(server);
            container.Register(database);
            container.Register<IMongoRepository<UserModel>>(new MongoRepository<UserModel>(database));
            container.Register<IUserMapper, UserDatabase>();
        }

        protected override void RequestStartup(TinyIoC.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines, NancyContext context)
        {
            const string defaultLang = "en";
            var user = (UserIdentity)context.CurrentUser;
            var ci = new CultureInfo(defaultLang);

            if (user != null)
            {
                if (user.CultureInfo != null)
                    ci = user.CultureInfo;
                else
                    user.CultureInfo = ci;
            }

            Thread.CurrentThread.CurrentUICulture = ci;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);

            var formsAuthConfiguration = new FormsAuthenticationConfiguration
            {
                RedirectUrl = "~/login",
                UserMapper = container.Resolve<IUserMapper>(),
            };

            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
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