using Microsoft.Build.Framework;

namespace Presentation.Areas.Admin.Models.TransactionTypeVM
{
    public class TransactionTypeVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
    }
}
