using System.Collections.Generic;
using System.Linq;
using PhotoCache.Web.Helpers;

namespace PhotoCache.Web.Modules
{
    public class BaseAPIModule : BaseModule
    {
        private dynamic _data;

        public BaseAPIModule() : base("/api")
        {
        }

        protected dynamic Data
        {
            get
            {
                if (_data != null) return _data;

                var contentHeader = Request.Headers.ContentType.Any()
                    ? Request.Headers["Content-Type"]
                    : null;

                if (contentHeader != null)
                {
                    var firstOrDefault = contentHeader.FirstOrDefault();
                    if (firstOrDefault != null)
                    {
                        string contentType = firstOrDefault.Split(';')[0];

                        if (contentType == null)
                            return null;
                        if (contentType.Contains("application/x-www-form-urlencoded"))
                            return _data = new CaseInsensitiveDynamicDictionary(Request.Form);
                        if (contentType.EndsWith("xml"))
                            return _data = Request.Body.FromXml<CaseInsensitiveDynamicDictionary>();
                        if (contentType.EndsWith("json"))
                            return _data = Request.Body.FromJson<CaseInsensitiveDynamicDictionary>();
                    }
                }
                return null;
            }
        }

        protected string[] Queries
        {
            get
            {
                var queries = new List<string>();

                foreach (var query in Request.Query)
                {
                    queries.Add(query);
                }

                return queries.ToArray();
            }
        }
    }
}