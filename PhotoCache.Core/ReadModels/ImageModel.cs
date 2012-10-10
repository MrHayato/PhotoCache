using System;

namespace PhotoCache.Core.ReadModels
{
    public class ImageModel : IModel
    {
        public Guid Id { get; set; }
        public string OriginalImageId { get; set; }
        public string ProcessedImageId { get; set; }
        public string OriginalImageThumbnailId { get; set; }
        public string ProcessedImageThumbnailId { get; set; }
    }
}
