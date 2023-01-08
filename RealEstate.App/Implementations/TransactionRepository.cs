using Microsoft.EntityFrameworkCore;
using RealEstate.App.Constants;
using RealEstate.App.Interfaces;
using RealEstate.Data.Context;
using RealEstate.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.App.Implementations
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        protected readonly DBRealEstateContext _db;

        public TransactionRepository(DBRealEstateContext db) : base(db)
        {
            _db = db;
        }

        public void CheckTransactionDate()
        {
            var transactions = _db.Transactions.Include(x => x.TransactionTypeNavigation).Where(x => x.TransactionTypeNavigation.Name == TransactionTypes.Rent).ToList();

            foreach (var transaction in transactions)
            {
                if (transaction.RentEndDate < DateTime.Now && transaction.Status!=TransactionStatus.Expired)
                {
                    transaction.Status = TransactionStatus.Expired;
                    var property = _db.Properties.Find(transaction.PropertyId);
                    if (property != null)
                    {
                        property.Status = PropertyStatus.Free;
                        _db.SaveChanges();
                    }
                }
            }
        }

        public string UpdateStatus(Transaction transaction, string status)
        {
            transaction.Status = status;
            return status;
        }
    }
}
