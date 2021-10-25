using AutoFixture;
using ConversionTool.HtmlToImage;
using ConversionTool.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace ConversionTool.Controllers
{
    public class HtmlToImageControllerTest
    {
        [Fact]
        public async Task Png_Should_return_a_png_image_file_content()
        {
            // Arrange
            var fixture = new Fixture();
            var html = fixture.Create<string>();
            var htmlAsByteArray = fixture.Create<byte[]>();

            var htmlToImageMock = new Mock<IHtmlToImage>();
            htmlToImageMock.Setup(x => x.FromStringToPngAsync(html, null, null))
                .ReturnsAsync(htmlAsByteArray);

            var sut = new HtmlToImageController(
                Mock.Of<ILogger<HtmlToImageController>>(),
                htmlToImageMock.Object);

            var payload = new HtmlToImageModel()
            {
                Html = html
            };

            // Act
            var response = await sut.Png(payload);
            var responseAction = response as FileContentResult;

            // Assert
            Assert.IsType<FileContentResult>(response);
            Assert.Equal(htmlAsByteArray, responseAction.FileContents);
            Assert.Equal("image/png", responseAction.ContentType);
        }
    }
}
