using EndoplasmCleanArchitecture.Authentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndoplasmCleanArchitecture.Authentication.Interfaces
{
    public  interface ITokenService
    {
        public Task<TokenResult> GenerateToken(AppUser user);
        Task<TokenResult> RefreshTokensAsync(RefreshTokenRequest tokenModel);
    }
}
