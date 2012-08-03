using System;
using Nancy;
using PhotoCache.Core.Models;
using PhotoCache.Core.Services;
using PhotoCache.Web.Helpers;

namespace PhotoCache.Web.Modules.APIModules
{
    public class ImageModule : BaseAPIModule
    {
        private readonly IModelService<ImageModel> _images;

        public ImageModule(IModelService<ImageModel> images)
        {
            _images = images;
            Get["/image/{id}"] = x => GetImage(x.id);
            Get["/images"] = x => GetImages();
        }

        private Response GetImages()
        {
            return Response.AsJson(_images.LoadAll());
        }

        private Response GetImage(Guid id)
        {
            var image = _images.Load(id);
            return image == null
                ? Response.Error("Image not found", HttpStatusCode.NotFound)
                : Response.AsJson(image);
        }
    }
}