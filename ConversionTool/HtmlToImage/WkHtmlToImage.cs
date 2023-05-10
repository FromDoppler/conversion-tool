using ConversionTool.Configuration;
using HtmlConverter.Configurations;
using HtmlConverter.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Threading.Tasks;

namespace ConversionTool.HtmlToImage
{
    public class WkHtmlToImage : IHtmlToImage
    {
        IAppConfiguration _appConfiguration;
        public WkHtmlToImage(IAppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
        }

        public Task<byte[]> FromStringToPngAsync(string html, int? height = null, int? width = null)
        {
            var imageConfiguration = new ImageConfiguration
            {
                Content = html,
                Format = ImageFormat.Png,
                Quality = 50,
            };
            var resizedImage = ResizeImage(HtmlConverter.Core.HtmlConverter.ConvertHtmlToImage(imageConfiguration), height, width);
            return Task.FromResult(resizedImage);
        }

        private byte[] ResizeImage(byte[] image, int? height, int? width)
        {
            using Image imageRgba = Image.Load(image);
            using MemoryStream stream = new MemoryStream();
            var withToMutate = width ?? imageRgba.Width * _appConfiguration.ImageSizeResult.Height / imageRgba.Height;
            var heightToMutate = height ?? _appConfiguration.ImageSizeResult.Height;

            imageRgba.Mutate(x => x.Resize(withToMutate, heightToMutate, KnownResamplers.Lanczos3));
            imageRgba.Save(stream, new PngEncoder());
            return stream.ToArray();
        }
    }
}
