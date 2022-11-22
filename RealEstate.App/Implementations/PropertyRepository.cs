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
    public class PropertyRepository : Repository<Property>, IPropertyRepository
    {
        protected readonly DBRealEstateContext _db;

        public PropertyRepository(DBRealEstateContext db) : base(db)
        {
            _db = db;
        }
    }
}
