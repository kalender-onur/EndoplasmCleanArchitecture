using EndoplasmCleanArchitecture.Application.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EndoplasmCleanArchitecture.Application.Interfaces.User
{
    public interface IUserService
    {
        Task<bool> CreateUserAsync(CreateUserRequestDto dto);
        Task<bool> InsertRefreshToken(string userName, string refreshToken, DateTime expiration);
        Task<Domain.Entities.User> GetUserByName(string userName);
    }
}
