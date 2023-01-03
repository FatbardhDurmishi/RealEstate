using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RealEstate.Data.Entities
{
    public partial class TransactionsType
    {
        public TransactionsType()
        {
            Properties = new HashSet<Property>();
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [InverseProperty("TransactionTypeNavigation")]
        public virtual ICollection<Property> Properties { get; set; }
        [InverseProperty("TransactionTypeNavigation")]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
