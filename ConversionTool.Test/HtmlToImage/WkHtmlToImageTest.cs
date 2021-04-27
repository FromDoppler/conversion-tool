using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ConversionTool.HtmlToImage
{
    public class WkHtmlToImageTest
    {
        [InlineData("<h1>title</h1><p>this is a html example</p>")]
        [InlineData("<div></div>")]
        [InlineData("<html></html>")]
        [Theory]
        public async Task FromStringToPngAsync_should_return_a_png_image(string htmlText)
        {
            // Arrange
            var sut = new WkHtmlToImage();
            // See https://en.wikipedia.org/wiki/Portable_Network_Graphics#File_header
            var pngBytesHeader = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

            // Act
            var bytes = await sut.FromStringToPngAsync(htmlText);

            // Assert
            Assert.Equal(pngBytesHeader, bytes.Take(8));
        }
    }
}
