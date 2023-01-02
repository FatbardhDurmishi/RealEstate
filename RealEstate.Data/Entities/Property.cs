using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RealEstate.Data.Entities
{
    public partial class Property
    {
        public Property()
        {
            PropertyImages = new HashSet<PropertyImage>();
            Transactions = new HashSet<Transaction>();
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
        public string Status { get; set; } = null!;
        [StringLength(50)]
        public string State { get; set; } = null!;
        [StringLength(50)]
        public string City { get; set; } = null!;
        [StringLength(50)]
        public string StreetAddress { get; set; } = null!;
        public string CoverImageUrl { get; set; } = null!;
        public int? TransactionType { get; set; }
        [StringLength(450)]
        public string? UserId { get; set; }
        public int? PropertyType { get; set; }

        [ForeignKey("PropertyType")]
        [InverseProperty("Properties")]
        public virtual PropertyType? PropertyTypeNavigation { get; set; }
        [ForeignKey("TransactionType")]
        [InverseProperty("Properties")]
        public virtual TransactionsType? TransactionTypeNavigation { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("Properties")]
        public virtual AspNetUser? User { get; set; }
        [InverseProperty("Property")]
        public virtual ICollection<PropertyImage> PropertyImages { get; set; }
        [InverseProperty("Property")]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
