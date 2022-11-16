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
    public class UserRepository : Repository<AspNetUser>, IUserRepository
    {
        protected readonly DBRealEstateContext _db;

        public UserRepository(DBRealEstateContext db) : base(db)
        {
            _db = db;
        }
        public AspNetUser? GetByStringId(string id)
        {
            return _db.AspNetUsers.FirstOrDefault(x => x.Id == id);
        }
    }
}
