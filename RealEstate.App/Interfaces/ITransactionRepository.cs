using RealEstate.Data.Context;
using RealEstate.Data.Entities;



namespace RealEstate.App.Interfaces
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        string UpdateStatus(Transaction transaction, string status);
        void CheckTransactionDate();

    }
}
