using System;
using System.Drawing;
using System.Linq;
using Nancy;
using PhotoCache.Core.Extensions;
using Raven.Client;

namespace PhotoCache.Web.Modules.APIModules
{
    public class AssetModule : BaseAPIModule
    {
        private readonly IDocumentStore _documentStore;

        public AssetModule(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
            Get["/asset/{id}"] = x => GetAsset(x.id);
            Post["/asset/process"] = x => ProcessAsset();
        }

        private Response GetAsset(Guid id)
        {
            var asset = _documentStore.DatabaseCommands.GetAttachment(id.ToString());
            var data = asset.Data().ToByteArray();

            var response = new Response();
            response.ContentType = "image/png";
            response.Contents = s => s.Write(data, 0, data.Length);
            response.StatusCode = HttpStatusCode.OK;

            return response;
        }

        private Response ProcessAsset()
        {
            var file = Request.Files.First();
            var bitmap = new Bitmap(file.Value);
            var response = new Response();
            byte[] bytes = bitmap.ToByteArray();

            response.StatusCode = HttpStatusCode.OK;
            response.ContentType = "image/png";
            response.Contents = stream => stream.Write(bytes, 0, bytes.Length);

            return response;
        }
    }
}