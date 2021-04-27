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

        public Task<byte[]> FromStringToPngAsync(string html)
        {
            var imageConfiguration = new ImageConfiguration
            {
                Content = html,
                Format = ImageFormat.Png,
                Quality = 50,
            };
            var resizedImage = ResizeImage(HtmlConverter.Core.HtmlConverter.ConvertHtmlToImage(imageConfiguration));
            return Task.FromResult(resizedImage);
        }

        private byte[] ResizeImage(byte[] image)
        {
            using (Image imageRgba = Image.Load(image))
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    var NewWidth = imageRgba.Width * _appConfiguration.ImageSizeResult.Height / imageRgba.Height;
                    imageRgba.Mutate(x => x.Resize(NewWidth, _appConfiguration.ImageSizeResult.Height, KnownResamplers.Lanczos3));
                    imageRgba.Save(stream, new PngEncoder());
                    return stream.ToArray();
                }
            }
        }
    }
}
