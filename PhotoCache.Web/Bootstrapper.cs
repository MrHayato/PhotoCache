using System;
using System.Globalization;
using System.Threading;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using FluentValidation;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Windsor;
using Nancy.Conventions;
using Nancy.Session;
using Nancy.Validation.FluentValidation;
using PhotoCache.Core.Models;
using PhotoCache.Core.Persistence;
using PhotoCache.Core.Services;
using PhotoCache.Web.Authentication;
using Raven.Client;
using Raven.Client.Document;

namespace PhotoCache.Web
{
    public class Bootstrapper : WindsorNancyBootstrapper
    {
        private IDocumentStore CreateDocumentStore()
        {
            var documentStore = new DocumentStore { ConnectionStringName = "RAVENHQ_CONNECTION_STRING" };
            documentStore.Initialize();

            var originalKeyGen = documentStore.Conventions.DocumentKeyGenerator;
            documentStore.Conventions.DocumentKeyGenerator = delegate(object obj)
                {
                    var userModel = obj as IModel;
                    return userModel != null ? Guid.NewGuid().ToString() : originalKeyGen(obj);
                };

            return documentStore;
        }

        protected override void ApplicationStartup(IWindsorContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            CookieBasedSessions.Enable(pipelines);
        }

        protected override void ConfigureApplicationContainer(IWindsorContainer container)
        {
            container.Register(Component.For<IFluentAdapterFactory>().ImplementedBy<DefaultFluentAdapterFactory>().LifestyleSingleton());
            container.Register(Component.For<IUserMapper>().ImplementedBy<UserDatabase>().LifestyleSingleton());
            container.Register(Component.For(typeof(IDocumentStore)).Instance(CreateDocumentStore()).LifestyleSingleton());
            container.Register(Component.For(typeof(IRavenRepository<>)).ImplementedBy(typeof(RavenRepository<>)).LifestyleSingleton());
            container.Register(Component.For(typeof(IModelService<>)).ImplementedBy(typeof(ModelService<>)).LifestyleSingleton());

            //Validators
            container.Register(Classes
                .FromAssemblyNamed("PhotoCache.Core")
                .BasedOn(typeof(IValidator<>))
                .WithService.Base()
                .LifestyleSingleton());
        }

        protected override void RequestStartup(IWindsorContainer container, IPipelines pipelines, NancyContext context)
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
                UserMapper = container.Resolve<IUserMapper>()
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

            nancyConventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("fonts", @"Content\fonts")
            );

            nancyConventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("templates", @"Content\templates")
            );
        }
    }
}