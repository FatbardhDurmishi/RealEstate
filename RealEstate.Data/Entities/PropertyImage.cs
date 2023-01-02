using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RealEstate.Data.Entities
{
    public partial class PropertyImage
    {
        [Key]
        public int Id { get; set; }
        public string ImageUrl { get; set; } = null!;
        public int? PropertyId { get; set; }

        [ForeignKey("PropertyId")]
        [InverseProperty("PropertyImages")]
        public virtual Property? Property { get; set; }
    }
}
