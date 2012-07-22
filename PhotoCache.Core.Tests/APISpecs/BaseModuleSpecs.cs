using System;
using System.Collections.Generic;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Machine.Specifications;
using Nancy;
using Nancy.Testing;
using Newtonsoft.Json;
using PhotoCache.Core.Extensions;
using PhotoCache.Core.Models;
using PhotoCache.Core.Persistence;
using PhotoCache.Core.Specs.Persistence;
using PhotoCache.Web;
using Raven.Client;
using Raven.Client.Embedded;

namespace PhotoCache.Core.Specs.APISpecs
{
    public class TestBootstrapper : Bootstrapper
    {
        protected override void ConfigureApplicationContainer(IWindsorContainer container)
        {
            base.ConfigureApplicationContainer(container);
            container.Register(Component.For(typeof(IRavenRepository<>)).ImplementedBy(typeof(FakeRavenRepository<>)).LifestyleSingleton());
        }

        protected override IDocumentStore CreateDocumentStore()
        {
            return new EmbeddableDocumentStore { RunInMemory = true }.Initialize();
        }
    }

    public class BaseModuleSpecs<T> where T : IModel
    {
        protected const string GET = "GET";
        protected const string PUT = "PUT";
        protected const string POST = "POST";
        protected const string DELETE = "DELETE";

        protected static BrowserResponse Response;
        protected static dynamic FormData;
        protected static Dictionary<string, string> Query;
        protected static Dictionary<string, string> Headers;

        private static readonly TestBootstrapper _bootstrapper = new TestBootstrapper();
        private static readonly Browser _browser = new Browser(_bootstrapper);
        private static string _body;

        protected static IRavenRepository<T> Repository = new FakeRavenRepository<T>(_bootstrapper.DocumentStore);

        private Establish context = () =>
        {
            Response = null;
            FormData = new DynamicDictionary();
            Query = new Dictionary<string, string>();
            Headers = new Dictionary<string, string>();
            _body = null;
        };

        protected static void JsonRequest(string location, object body = null, string method = GET)
        {
            Headers.AddOrUpdate("Content-Type", "application/json");
            _body = JsonConvert.SerializeObject(body);
            SendRequest(location, method);
        }

        protected static void Request(string location, string method = GET)
        {
            Headers.AddOrUpdate("Content-Type", "application/x-www-form-urlencoded");
            _body = FormEncoded(FormData);
            SendRequest(location, method);
        }

        private static void SendRequest(string location, string method)
        {
            location = "/api" + location;
            switch (method.ToUpper())
            {
                case GET:
                    Response = _browser.Get(location, SetContext);
                    break;
                case PUT:
                    Response = _browser.Put(location, SetContext);
                    break;
                case POST:
                    Response = _browser.Post(location, SetContext);
                    break;
                case DELETE:
                    Response = _browser.Delete(location, SetContext);
                    break;
                default:
                    throw new ArgumentException("Invalid method argument " + method, "method");
            }
        }

        private static void SetContext(BrowserContext context)
        {
            context.HttpRequest();
            context.Body(_body);
            foreach (var header in Headers) context.Header(header.Key, header.Value);
            foreach (var query in Query) context.Query(query.Key, query.Value);
        }

        private static string FormEncoded(DynamicDictionary dictionary)
        {
            var sb = new StringBuilder();
            foreach (var name in dictionary.GetDynamicMemberNames())
                sb.Append(name + '=' + dictionary[name].ToString() + '&');
            return sb.ToString().TrimEnd('&');
        }
    }
}
