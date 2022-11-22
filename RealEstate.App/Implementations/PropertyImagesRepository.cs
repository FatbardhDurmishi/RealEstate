using RealEstate.App.Interfaces;
using RealEstate.Data.Context;
using RealEstate.Data.Entities;

namespace RealEstate.App.Implementations
{
    public class PropertyImagesRepository : Repository<PropertyImage>, IPropertyImagesRepository
    {
        protected readonly DBRealEstateContext _db;

        public PropertyImagesRepository(DBRealEstateContext db) : base(db)
        {
            _db = db;
        }
    }
}

