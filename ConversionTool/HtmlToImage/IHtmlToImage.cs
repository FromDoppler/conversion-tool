using System.Threading.Tasks;

namespace ConversionTool.HtmlToImage
{
    public interface IHtmlToImage
    {
        Task<byte[]> FromStringToPngAsync(string html);
    }
}
