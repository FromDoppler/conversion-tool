using System.ComponentModel.DataAnnotations;

namespace ConversionTool.Model
{
    public class HtmlToImageModel
    {
        [Required]
        public string Html { get; set; }
        public int? Height { get; set; }
        public int? Width { get; set; }
    }
}
