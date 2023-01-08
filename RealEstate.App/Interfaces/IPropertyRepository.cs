using RealEstate.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.App.Interfaces
{
    public interface IPropertyRepository : IRepository<Property>
    {
        string UpdateStatus(Property property, string status);
    }
}
