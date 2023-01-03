using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.App.Interfaces
{
    public interface IUserService
    {
        string GetUserId();
        string GetUserName();
        string GetUserEmail();
        string GetUserPhoneNumber();
        string GetUserRole();
        string GetFullName();
        string GetUserCompanyId();
    }
}
