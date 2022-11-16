using Microsoft.EntityFrameworkCore;
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
    public class RolesRepository : IRolesRepository
    {
        protected readonly DBRealEstateContext _db;

        public RolesRepository(DBRealEstateContext db)
        {
            _db = db;
        }

        public AspNetRole? GetByUserId(string userId)
        {
            return _db.AspNetUsers.Include(x => x.Roles).FirstOrDefault(x => x.Id == userId)?.Roles.FirstOrDefault();
        }
    }
}
