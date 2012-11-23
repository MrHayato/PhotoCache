using Nancy;
using Nancy.Extensions;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using Nancy.Routing;
using Nancy.Validation;
using Nancy.ViewEngines;

namespace PhotoCache.Web.Modules
{

    /// <summary>
    /// Default implementation for building a full configured <see cref="NancyModule"/> instance.
    /// </summary>
    public class DefaultModuleBuilder : INancyModuleBuilder
    {
        private readonly IViewFactory _viewFactory;
        private readonly IResponseFormatterFactory _responseFormatterFactory;
        private readonly IModelBinderLocator _modelBinderLocator;
        private readonly IModelValidatorLocator _validatorLocator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultNancyModuleBuilder"/> class.
        /// </summary>
        /// <param name="viewFactory">The <see cref="IViewFactory"/> instance that should be assigned to the module.</param>
        /// <param name="responseFormatterFactory">An <see cref="IResponseFormatterFactory"/> instance that should be used to create a response formatter for the module.</param>
        /// <param name="modelBinderLocator">A <see cref="IModelBinderLocator"/> instance that should be assigned to the module.</param>
        /// <param name="validatorLocator">A <see cref="IModelValidatorLocator"/> instance that should be assigned to the module.</param>
        public DefaultModuleBuilder(IViewFactory viewFactory, IResponseFormatterFactory responseFormatterFactory, IModelBinderLocator modelBinderLocator, IModelValidatorLocator validatorLocator)
        {
            _viewFactory = viewFactory;
            _responseFormatterFactory = responseFormatterFactory;
            _modelBinderLocator = modelBinderLocator;
            _validatorLocator = validatorLocator;
        }

        /// <summary>
        /// Builds a fully configured <see cref="NancyModule"/> instance, based upon the provided <paramref name="module"/>.
        /// </summary>
        /// <param name="module">The <see cref="NancyModule"/> that shoule be configured.</param>
        /// <param name="context">The current request context.</param>
        /// <returns>A fully configured <see cref="NancyModule"/> instance.</returns>
        public NancyModule BuildModule(NancyModule module, NancyContext context)
        {
            CreateNegotiationContext(module, context);

            module.Context = context;
            module.Response = _responseFormatterFactory.Create(context);
            module.ViewFactory = _viewFactory;
            module.ModelBinderLocator = _modelBinderLocator;
            module.ValidatorLocator = _validatorLocator;

            return module;
        }

        private static void CreateNegotiationContext(NancyModule module, NancyContext context)
        {
            // TODO - not sure if this should be here or not, but it'll do for now :)
            context.NegotiationContext = new NegotiationContext
                                             {
                                                 ModuleName = module.GetModuleName(),
                                                 ModulePath = module.ModulePath,
                                             };
        }
 
    }
}