using System.Drawing;
using AForge.Imaging.Filters;
using PhotoCache.Core.Extensions;

namespace PhotoCache.Core.Imaging
{
    public static class ImageProcessor
    {
        public static Bitmap ApplyFilters(Bitmap image)
        {
            return image
                .Apply(new Grayscale(.5, .5, .5))
                .ApplyInPlace(new SobelEdgeDetector())
                .Apply(new FillHoles())
                .Apply(new ExtractBiggestBlob());
        }

    }
}
