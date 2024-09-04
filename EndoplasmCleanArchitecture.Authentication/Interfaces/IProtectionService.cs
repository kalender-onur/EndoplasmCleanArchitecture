using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndoplasmCleanArchitecture.Authentication.Interfaces
{
    public interface IProtectionService
    {
        string Protect(string plainText);
        string Unprotect(string protectedText);
    }
}
