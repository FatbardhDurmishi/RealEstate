using RealEstate.App.Interfaces;
using RealEstate.Data.Context;
using RealEstate.Data.Entities;

namespace RealEstate.App.Implementations
{
    public class PropertyRepository : Repository<Property>, IPropertyRepository
    {
        protected readonly DBRealEstateContext _db;

        public PropertyRepository(DBRealEstateContext db) : base(db)
        {
            _db = db;
        }

        public string UpdateStatus(Property property, string status)
        {
            property.Status = status;
            return status;
        }
    }
}
