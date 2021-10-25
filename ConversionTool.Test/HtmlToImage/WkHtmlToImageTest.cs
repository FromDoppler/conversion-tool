using ConversionTool.Configuration;
using ConversionTool.Configuration.Properties;
using Moq;
using SixLabors.ImageSharp;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ConversionTool.HtmlToImage
{
    public class WkHtmlToImageTest
    {
        private readonly Mock<IAppConfiguration> _appConfig;
        private readonly int _defaultImageHeight = 341;

        public WkHtmlToImageTest()
        {
            _appConfig = new Mock<IAppConfiguration>();
            _appConfig.Setup(x => x.ImageSizeResult).Returns(new ImageSizeResult
            {
                Height = _defaultImageHeight
            });
        }

        [InlineData("<h1>title</h1><p>this is a html example</p>")]
        [InlineData("<div></div>")]
        [InlineData("<html></html>")]
        [Theory]
        public async Task FromStringToPngAsync_should_return_a_png_image(string htmlText)
        {
            // Arrange
            var sut = new WkHtmlToImage(_appConfig.Object);
            // See https://en.wikipedia.org/wiki/Portable_Network_Graphics#File_header
            var pngBytesHeader = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

            // Act
            var bytes = await sut.FromStringToPngAsync(htmlText);

            // Assert
            Assert.Equal(pngBytesHeader, bytes.Take(8));
        }

        [InlineData("<h1>title</h1><p>this is a html example</p>", 50, 60)]
        [Theory]
        public async Task FromStringToPngAsync_should_return_the_size_required(string htmlText, int height, int width)
        {
            // Arrange
            var sut = new WkHtmlToImage(_appConfig.Object);

            // Act
            byte[] imageData = await sut.FromStringToPngAsync(htmlText, height, width);
            using var imageLoaded = Image.Load(imageData);

            // Assert
            Assert.Equal(height, imageLoaded.Height);
            Assert.Equal(width, imageLoaded.Width);
        }

        [InlineData("<h1>title</h1><p>this is a html example</p>")]
        [Theory]
        public async Task FromStringToPngAsync_should_return_the_default_size(string htmlText)
        {
            // Arrange
            var sut = new WkHtmlToImage(_appConfig.Object);

            // Act
            byte[] imageData = await sut.FromStringToPngAsync(htmlText);

            using var imageLoaded = Image.Load(imageData);

            // Assert
            Assert.Equal(_defaultImageHeight, imageLoaded.Height);

        }
    }
}
