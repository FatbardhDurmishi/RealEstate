using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Data.Entities
{
    public class PropertyImages
    {
        public int Id { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        [ForeignKey("Property")]
        public int PropertyId { get; set; }
        [ValidateNever]
        public Property Property { get; set; }
    }
}
