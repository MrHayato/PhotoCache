using System;
using System.Drawing;
using System.IO;
using Machine.Specifications;
using PhotoCache.Core.Imaging;

namespace PhotoCache.Core.Specs.ImagingSpecs
{
    public class ImageProcessorSpecs
    {
        protected static string _sourceDirectory = Environment.CurrentDirectory + @"\_assets";
        protected static string _targetDirectory = Environment.CurrentDirectory + @"\_assets\processed";

        private Establish that = () =>
                                 {
                                     if (!Directory.Exists(_sourceDirectory))
                                         Directory.CreateDirectory(_sourceDirectory);
                                     if (!Directory.Exists(_targetDirectory))
                                         Directory.CreateDirectory(_targetDirectory);

                                     var source = new DirectoryInfo(_sourceDirectory);
                                     var target = new DirectoryInfo(_targetDirectory);

                                     foreach (var file in target.GetFiles())
                                     {
                                         file.Delete();
                                     }

                                     foreach (var file in source.GetFiles())
                                     {
                                         Bitmap image = new Bitmap(file.FullName);
                                         image = ImageProcessor.ApplyFilters(image);
                                         image.Save(_targetDirectory + "\\" + file.Name);
                                     }
                                 };

        private It should_process_all_of_the_test_images = () => new DirectoryInfo(_sourceDirectory).GetFiles().Length.ShouldEqual(
            new DirectoryInfo(_targetDirectory).GetFiles().Length);
    }
}
