using Castle.Facilities.TypedFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Lifestyle.Scoped;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Nancy;
using Nancy.Bootstrappers.Windsor;
using Nancy.Diagnostics;
using Nancy.Bootstrapper;

namespace PhotoCache.Web
{
    /// <summary>
    /// Nancy bootstrapper for the Windsor container.
    /// </summary>
    public abstract class ModifiedWindsorNancyBootstrapper : NancyBootstrapperBase<IWindsorContainer>
    {
        private bool modulesRegistered;

        /// <summary>
        /// Gets the diagnostics for intialisation
        /// </summary>
        /// <returns>IDiagnostics implementation</returns>
        protected override IDiagnostics GetDiagnostics()
        {
            return ApplicationContainer.Resolve<IDiagnostics>();
        }

        /// <summary>
        /// Get all NancyModule implementation instances
        /// </summary>
        /// <param name="context">The current context</param>
        /// <returns>An <see cref="IEnumerable{T}"/> instance containing <see cref="NancyModule"/> instances.</returns>
        public override IEnumerable<NancyModule> GetAllModules(NancyContext context)
        {
            var currentScope =
                CallContextLifetimeScope.ObtainCurrentScope();

            if (currentScope != null)
            {
                return ApplicationContainer.ResolveAll<NancyModule>();
            }

            using (ApplicationContainer.BeginScope())
            {
                return ApplicationContainer.ResolveAll<NancyModule>();
            }
        }

        /// <summary>
        /// Gets the application level container
        /// </summary>
        /// <returns>Container instance</returns>
        protected override IWindsorContainer GetApplicationContainer()
        {
            if (ApplicationContainer != null)
            {
                return ApplicationContainer;
            }

            var container = new WindsorContainer();

            container.AddFacility<TypedFactoryFacility>();
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel, true));
            container.Register(Component.For<IWindsorContainer>().Instance(container));
            container.Register(Component.For<NancyRequestScopeInterceptor>());
            container.Kernel.ProxyFactory.AddInterceptorSelector(new NancyRequestScopeInterceptorSelector());

            return container;
        }

        /// <summary>
        /// Gets all registered application registration tasks
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> instance containing <see cref="IApplicationRegistrations"/> instances.</returns>
        protected override IEnumerable<IApplicationRegistrations> GetApplicationRegistrationTasks()
        {
            return ApplicationContainer.ResolveAll<IApplicationRegistrations>();
        }

        /// <summary>
        /// Gets all registered application startup tasks
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> instance containing <see cref="IApplicationStartup"/> instances.</returns>
        protected override IEnumerable<IApplicationStartup> GetApplicationStartupTasks()
        {
            return ApplicationContainer.ResolveAll<IApplicationStartup>();
        }

        /// <summary>
        /// Resolve INancyEngine
        /// </summary>
        /// <returns>INancyEngine implementation</returns>
        protected override INancyEngine GetEngineInternal()
        {
            return ApplicationContainer.Resolve<INancyEngine>();
        }

        /// <summary>
        /// Retrieves a specific <see cref="NancyModule"/> implementation based on its key
        /// </summary>
        /// <param name="moduleKey">Module key</param>
        /// <param name="context">The current context</param>
        /// <returns>The <see cref="NancyModule"/> instance that was retrived by the <paramref name="moduleKey"/> parameter.</returns>
        public override NancyModule GetModuleByKey(string moduleKey, NancyContext context)
        {
            var currentScope =
                CallContextLifetimeScope.ObtainCurrentScope();

            if (currentScope != null)
            {
                return ApplicationContainer.Resolve<NancyModule>(moduleKey);
            }

            using (ApplicationContainer.BeginScope())
            {
                return ApplicationContainer.Resolve<NancyModule>(moduleKey);
            }
        }

        /// <summary>
        /// Get the moduleKey generator
        /// </summary>
        /// <returns>IModuleKeyGenerator instance</returns>
        protected override IModuleKeyGenerator GetModuleKeyGenerator()
        {
            return ApplicationContainer.Resolve<IModuleKeyGenerator>();
        }

        /// <summary>
        /// Nancy internal configuration
        /// </summary>
        protected override NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                return NancyInternalConfiguration.WithOverrides(c =>
                {
                    c.ModuleKeyGenerator = typeof(WindsorModuleKeyGenerator);
                });
            }
        }

        /// <summary>
        /// Register the given module types into the container
        /// </summary>
        /// <param name="container">Container to register into</param>
        /// <param name="moduleRegistrationTypes">NancyModule types</param>
        protected override void RegisterModules(IWindsorContainer container, IEnumerable<ModuleRegistration> moduleRegistrationTypes)
        {
            if (modulesRegistered)
            {
                return;
            }

            modulesRegistered = true;

            var components = moduleRegistrationTypes.Select(r => Component.For(typeof(NancyModule))
                .ImplementedBy(r.ModuleType).Named(r.ModuleKey).LifeStyle.Scoped<NancyPerWebRequestScopeAccessor>())
                .Cast<IRegistration>().ToArray();

            ApplicationContainer.Register(components);
        }

        /// <summary>
        /// Register the bootstrapper's implemented types into the container.
        /// This is necessary so a user can pass in a populated container but not have
        /// to take the responsibility of registering things like INancyModuleCatalog manually.
        /// </summary>
        /// <param name="applicationContainer">Application container to register into</param>
        protected override void RegisterBootstrapperTypes(IWindsorContainer applicationContainer)
        {
            applicationContainer.Register(Component.For<INancyModuleCatalog>().Instance(this));
        }

        /// <summary>
        ///   Register the default implementations of internally used types into the container as singletons
        /// </summary>
        /// <param name="container"> Container to register into </param>
        /// <param name="typeRegistrations"> Type registrations to register </param>
        protected override void RegisterTypes(IWindsorContainer container, IEnumerable<TypeRegistration> typeRegistrations)
        {
            foreach (var typeRegistration in typeRegistrations)
            {
                RegisterNewOrAddService(container, typeRegistration.RegistrationType, typeRegistration.ImplementationType);
            }
        }

        /// <summary>
        ///   Register the various collections into the container as singletons to later be resolved by IEnumerable{Type} constructor dependencies.
        /// </summary>
        /// <param name="container"> Container to register into </param>
        /// <param name="collectionTypeRegistrations"> Collection type registrations to register </param>
        protected override void RegisterCollectionTypes(IWindsorContainer container, IEnumerable<CollectionTypeRegistration> collectionTypeRegistrations)
        {
            foreach (var typeRegistration in collectionTypeRegistrations)
            {
                foreach (var implementationType in typeRegistration.ImplementationTypes)
                {
                    RegisterNewOrAddService(container, typeRegistration.RegistrationType, implementationType);
                }
            }
        }

        /// <summary>
        /// Register the given instances into the container
        /// </summary>
        /// <param name="container">Container to register into</param>
        /// <param name="instanceRegistrations">Instance registration types</param>
        protected override void RegisterInstances(IWindsorContainer container, IEnumerable<InstanceRegistration> instanceRegistrations)
        {
            foreach (var instanceRegistration in instanceRegistrations)
            {
                container.Register(Component.For(instanceRegistration.RegistrationType)
                    .Instance(instanceRegistration.Implementation));
            }
        }

        private static void RegisterNewOrAddService(IWindsorContainer container, Type registrationType, Type implementationType)
        {
            var handler = container.Kernel.GetHandler(implementationType);
            if (handler != null)
            {
                handler.ComponentModel.AddService(registrationType);
                return;
            }

            container.Register(Component.For(implementationType, registrationType).ImplementedBy(implementationType));
        }
    }
}