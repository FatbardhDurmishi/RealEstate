using Microsoft.Build.Framework;

namespace Presentation.Areas.Admin.Models.PropertyTypeVM
{
    public class PropertyTypeVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
    }
}
