using EndoplasmCleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace EndoplasmCleanArchitecture.Authentication.Models
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {

        }
        public AppUser(User user)
        {
            this.Id = user.Id.ToString();
            this.UserName = user.UserName;
            this.Password = user.Password;
            this.RefreshToken = user.RefreshToken;
            if (user.RefreshTokenExpiryTime.HasValue)
                this.RefreshTokenExpiryTime = user.RefreshTokenExpiryTime.Value;

        }
        public string Password { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
