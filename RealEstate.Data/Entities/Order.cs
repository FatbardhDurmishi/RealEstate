using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateOnly RentStartDate { get; set; }
        public DateOnly RentEndDate { get; set; }
        [ForeignKey("Property")]
        public int PropertyId { get; set; }
        public Property Property { get; set; }
        [ForeignKey("AspNetUser")]
        public int UserId { get; set; }
        public AspNetUser AspNetUser { get; set; }
    }
}
