using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using AForge.Imaging.Filters;

namespace PhotoCache.Core.Extensions
{
    public static class ImageExtensions
    {
        //Easier to chain filters
        public static Bitmap ApplyInPlace(this Bitmap bitmap, IInPlaceFilter filter)
        {
            filter.ApplyInPlace(bitmap);
            return bitmap;
        }

        public static Bitmap Apply(this Bitmap bitmap, IFilter filter)
        {
            return filter.Apply(bitmap);
        }

        public static Bitmap GenerateThumbnail(this Bitmap image, int width, int height)
        {
            return image
                .Apply(new ResizeBicubic(width, height));
        }

        public static byte[] ToByteArray(this Bitmap bitmap)
        {
            byte[] bytes;

            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                bytes = ms.GetBuffer();
            }

            return bytes;
        }

        public static byte[] ToByteArray(this Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
