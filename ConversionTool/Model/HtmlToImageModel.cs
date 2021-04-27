using System.ComponentModel.DataAnnotations;

namespace ConversionTool.Model
{
    public class HtmlToImageModel
    {
        [Required]
        public string Html { get; set; }
    }
}
