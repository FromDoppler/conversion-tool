using HtmlConverter.Configurations;
using HtmlConverter.Options;
using System.Threading.Tasks;

namespace ConversionTool.HtmlToImage
{
    public class WkHtmlToImage : IHtmlToImage
    {
        public Task<byte[]> FromStringToPngAsync(string html)
        {
            var imageConfiguration = new ImageConfiguration
            {
                Content = html,
                Format = ImageFormat.Png,
            };
            var image = HtmlConverter.Core.HtmlConverter.ConvertHtmlToImage(imageConfiguration);
            return Task.FromResult(image);
        }
    }
}
