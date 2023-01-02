using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RealEstate.Data.Entities
{
    public partial class Transaction
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }
        [StringLength(450)]
        public string? OwnerId { get; set; }
        [StringLength(450)]
        public string? BuyerId { get; set; }
        public int? PropertyId { get; set; }
        [Column(TypeName = "datetime")]
        [Required]
        public DateTime? RentStartDate { get; set; }
        [Column(TypeName = "datetime")]
        [Required]
        public DateTime? RentEndDate { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? RentPrice { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TotalPrice { get; set; }
        [StringLength(50)]
        public string? Status { get; set; }

        [ForeignKey("BuyerId")]
        [InverseProperty("TransactionBuyers")]
        public virtual AspNetUser? Buyer { get; set; }
        [ForeignKey("OwnerId")]
        [InverseProperty("TransactionOwners")]
        public virtual AspNetUser? Owner { get; set; }
        [ForeignKey("PropertyId")]
        [InverseProperty("Transactions")]
        public virtual Property? Property { get; set; }
    }
}
