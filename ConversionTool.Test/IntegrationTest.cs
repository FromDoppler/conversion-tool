using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ConversionTool
{
    public class IntegrationTest
        : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public IntegrationTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/swagger", HttpStatusCode.Moved, null)]
        [InlineData("/swagger/index.html", HttpStatusCode.OK, "text/html; charset=utf-8")]
        [InlineData("/robots.txt", HttpStatusCode.OK, "text/plain")]
        [InlineData("/favicon.ico", HttpStatusCode.OK, "image/x-icon")]
        [InlineData("/swagger/v1/swagger.json", HttpStatusCode.OK, "application/json; charset=utf-8")]
        [InlineData("/", HttpStatusCode.NotFound, null)]
        [InlineData("/Not/Found", HttpStatusCode.NotFound, null)]
        public async Task GET_endpoints_return_correct_status_and_contentType(string url, HttpStatusCode expectedStatusCode, string expectedContentType)
        {
            // Arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions()
            {
                AllowAutoRedirect = false
            });

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
            Assert.Equal(expectedContentType, response.Content?.Headers?.ContentType?.ToString());
        }
    }
}
