using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndoplasmCleanArchitecture.Authentication.Models
{
    public class TokenResult
    {
        public string Token { get; set; } 
        public string RefreshToken { get; set; } 
        public DateTime Expiration { get; set; } 
        public string ErrorMessage { get; set; } 
    }
}
