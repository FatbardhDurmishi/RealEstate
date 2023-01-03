using RealEstate.Data.Entities;

namespace Presentation.Areas.Company.Models.TransactionVM
{
    public class TransactionVM
    {
        public Property? Property { get; set; }
        public Transaction? Transaction { get; set; }
    }
}
