using RealEstate.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Presentation.Areas.Company.Models.PropertyVM
{
    public class PropertyVM
    {
        public Property Property { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> TransactionList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> UsersList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> PropertyTypeList { get; set; }
        [ValidateNever]
        [DisplayName("Choose the cover Image of your property")]
        public IFormFile CoverImage { get; set; }
        [ValidateNever]
        //public List<PropertyImage> PopertyImage { get; set; }
        [DisplayName("Choose the Images of your Property")]
        public IFormFileCollection PropertyImages { get; set; }
        [ValidateNever]
        [BindNever]
        public int[] DeleteImageIdArr { get; set; } =new int[0];
        //[DisplayName("Choose the Images of your property")]
        //public List<PropertyImageVM> Images { get; set; }

    }
}
//[Key]
//public int Id { get; set; }
//[StringLength(50)]
//public string Name { get; set; } = null!;
//public string Description { get; set; } = null!;
//public int? BedRooms { get; set; }
//public int? BathRooms { get; set; }
//[Column(TypeName = "decimal(18, 2)")]
//public decimal Area { get; set; }
//[Column(TypeName = "decimal(18, 2)")]
//public decimal Price { get; set; }
//[StringLength(50)]
//public string Status { get; set; } = null!;
//[StringLength(50)]
//public string State { get; set; } = null!;
//[StringLength(50)]
//public string City { get; set; } = null!;
//[StringLength(50)]
//public string StreetAddress { get; set; } = null!;
//public string CoverImageUrl { get; set; } = null!;
//public int? TransactionType { get; set; }
//[StringLength(450)]
//public string? UserId { get; set; }
//public int? PropertyType { get; set; }

//[ForeignKey("PropertyType")]
//[InverseProperty("Properties")]
//public virtual PropertyType? PropertyTypeNavigation { get; set; }
//[ForeignKey("TransactionType")]
//[InverseProperty("Properties")]
//public virtual TransactionsType? TransactionTypeNavigation { get; set; }
//[ForeignKey("UserId")]
//[InverseProperty("Properties")]
//public virtual AspNetUser? User { get; set; }
//[InverseProperty("Property")]
//public virtual ICollection<PropertyImage> PropertyImages { get; set; }