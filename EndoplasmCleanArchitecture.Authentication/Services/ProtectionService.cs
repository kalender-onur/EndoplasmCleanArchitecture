using EndoplasmCleanArchitecture.Authentication.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndoplasmCleanArchitecture.Authentication.Services
{
    public class ProtectionService : IProtectionService
    {
        private readonly IDataProtector _protector;

        public ProtectionService(IDataProtectionProvider dataProtectionProvider)
        {
            _protector = dataProtectionProvider.CreateProtector("EndoplasmCleanArchitecture.DataProtection");
        }

        public string Protect(string plainText)
        {
            return _protector.Protect(plainText);
        }

        public string Unprotect(string protectedText)
        {
            try
            {
                return _protector.Unprotect(protectedText);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Geçersiz veri.");
            }
        }
    }
}
