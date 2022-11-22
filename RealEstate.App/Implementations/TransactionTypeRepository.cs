using RealEstate.App.Interfaces;
using RealEstate.Data.Context;
using RealEstate.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.App.Implementations
{
    public class TransactionTypeRepository : Repository<TransactionsType>, ITransactionTypeRepository
    {
        protected readonly DBRealEstateContext _db;

        public TransactionTypeRepository(DBRealEstateContext db) : base(db)
        {
            _db = db;
        }
    }
}

