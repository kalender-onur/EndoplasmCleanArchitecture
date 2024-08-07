using EndoplasmCleanArchitecture.Authentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndoplasmCleanArchitecture.Authentication.Interfaces
{
    public interface IAppUserService
    {
        Task<AppUser> Authenticate(string username, string password);
        Task<AppUser> GetUserByName(string username);
    }
}
