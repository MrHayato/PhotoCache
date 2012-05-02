using System;
using System.Configuration;
using System.Globalization;
using System.Threading;
using Cassette.Nancy;
using FluentValidation;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.Session;
using PhotoCache.Core.Models;
using PhotoCache.Core.Persistence;
using PhotoCache.Core.Validators;
using PhotoCache.Web.Authentication;
using Raven.Client;
using Raven.Client.Document;
using TinyIoC;

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

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            CookieBasedSessions.Enable(pipelines);
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            var documentStore = new DocumentStore { ConnectionStringName = "RAVENHQ_CONNECTION_STRING" };
            documentStore.Initialize();

            var originalKeyGen = documentStore.Conventions.DocumentKeyGenerator;
            documentStore.Conventions.DocumentKeyGenerator = delegate(object obj)
                {
                    var userModel = obj as IModel;

                    return userModel != null ? Guid.NewGuid().ToString() : originalKeyGen(obj);
                };

            var userRepo = new RavenRepository<UserModel>(documentStore);

            container.Register<IDocumentStore>(documentStore);
            container.Register<IUserMapper, UserDatabase>();
            container.Register<IRavenRepository<UserModel>>(userRepo);
            container.Register<IValidator<UserModel>>(new UserModelValidator(userRepo));
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            const string defaultLang = "en";
            var currentUser = context.CurrentUser as UserIdentity;
            var user = currentUser != null ? currentUser.User : null;
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