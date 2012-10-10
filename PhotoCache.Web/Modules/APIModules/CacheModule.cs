using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Nancy;
using PhotoCache.Core.Extensions;
using PhotoCache.Core.Imaging;
using PhotoCache.Core.ReadModels;
using PhotoCache.Core.Services;
using PhotoCache.Web.Helpers;
using Raven.Client;
using Raven.Json.Linq;

namespace PhotoCache.Web.Modules.APIModules
{
    public class CacheModule : BaseAPIModule
    {
        private readonly IModelService<ImageModel> _images;
        private readonly IModelService<CacheModel> _caches;
        private readonly IDocumentStore _documentStore;

        public CacheModule(IModelService<ImageModel> images, IModelService<CacheModel> caches, IDocumentStore documentStore)
        {
            _images = images;
            _caches = caches;
            _documentStore = documentStore;
            Post["/cache"] = x => CreateCache();
            Get["/caches"] = x => GetCaches();
            Get["/cache/{id}"] = x => GetCache(x.id);
        }

        private Response GetCache(Guid id)
        {
            var cache = _caches.Load(id);
            return cache == null
                       ? Response.Error("Cache not found", HttpStatusCode.NotFound)
                       : Response.AsJson(cache);
        }

        private Response GetCaches()
        {
            return Response.AsJson(_caches.LoadAll());
        }

        private Response CreateCache()
        {
            var cache = new CacheModel();
            var imageModel = new ImageModel();

            var file = Request.Files.First();
            var original = new Bitmap(file.Value);
            var processed = ImageProcessor.ApplyFilters(original);

            imageModel.Id = Guid.NewGuid();
            imageModel.OriginalImageId = Guid.NewGuid().ToString();
            imageModel.OriginalImageThumbnailId = Guid.NewGuid().ToString();
            imageModel.ProcessedImageId = Guid.NewGuid().ToString();
            imageModel.ProcessedImageThumbnailId = Guid.NewGuid().ToString();

            cache.Id = Guid.NewGuid();
            cache.Images = new List<Guid>(new []{ imageModel.Id });
            cache.CreatorId = Guid.Empty;
            cache.CreatedDate = DateTime.Now;

            using (var ms = new MemoryStream(original.ToByteArray()))
                _documentStore.DatabaseCommands.PutAttachment(imageModel.OriginalImageId, null, ms, new RavenJObject());

            using (var ms = new MemoryStream(processed.ToByteArray()))
                _documentStore.DatabaseCommands.PutAttachment(imageModel.ProcessedImageId, null, ms, new RavenJObject());

            using (var ms = new MemoryStream(original.GenerateThumbnail(80, 80).ToByteArray()))
                _documentStore.DatabaseCommands.PutAttachment(imageModel.OriginalImageThumbnailId, null, ms, new RavenJObject());

            using (var ms = new MemoryStream(processed.GenerateThumbnail(80, 80).ToByteArray()))
                _documentStore.DatabaseCommands.PutAttachment(imageModel.ProcessedImageThumbnailId, null, ms, new RavenJObject());

            _images.Create(imageModel);
            _caches.Create(cache);

            return Response.AsJson(cache);
        }
    }
}