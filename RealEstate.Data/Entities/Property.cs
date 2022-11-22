using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

namespace RealEstate.Data.Entities
{
    public partial class Property
    {
        public Property()
        {
            PropertyImages = new HashSet<PropertyImage>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int? BedRooms { get; set; }
        public int? BathRooms { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Area { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        [StringLength(50)]
        public string? Status { get; set; }
        [StringLength(50)]
        public string State { get; set; } = null!;
        [StringLength(50)]
        public string City { get; set; } = null!;
        [StringLength(50)]
        public string StreetAddress { get; set; } = null!;
        [DisplayName("Choose the cover Image of your property")]
        [ValidateNever]
        public string? CoverImageUrl { get; set; }
        public int? TransactionType { get; set; }
        [StringLength(450)]
        public string? UserId { get; set; }
        public int? PropertyType { get; set; }
        [DisplayName("Choose the type of the Property")]
        [ForeignKey("PropertyType")]
        [InverseProperty("Properties")]
        [ValidateNever]
        public virtual PropertyType? PropertyTypeNavigation { get; set; }
        [DisplayName("Choose the type of the Transaction")]
        [ForeignKey("TransactionType")]
        [InverseProperty("Properties")]
        [ValidateNever]
        public virtual TransactionsType? TransactionTypeNavigation { get; set; }
        [DisplayName("Choose the Agent you want to assing the property to")]
        [ForeignKey("UserId")]
        [InverseProperty("Properties")]
        [ValidateNever]
        public virtual AspNetUser? User { get; set; }
        [ValidateNever]
        [DisplayName("Choose the Images of your Property")]
        [InverseProperty("Property")]
        public virtual ICollection<PropertyImage> PropertyImages { get; set; }
    }
}
