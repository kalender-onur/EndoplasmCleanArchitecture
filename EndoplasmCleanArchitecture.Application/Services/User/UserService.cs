using EndoplasmCleanArchitecture.Application.Dtos.User;
using EndoplasmCleanArchitecture.Application.Interfaces;
using EndoplasmCleanArchitecture.Application.Interfaces.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndoplasmCleanArchitecture.Application.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
        public async Task<bool> CreateUserAsync(CreateUserRequestDto dto)
        {

            if (await _unitOfWork.UserRepository.IsUserExists(dto.UserName))
                throw new Exception($"{dto.UserName} adlı kullanıcı sistemde mevcut");

            await _unitOfWork.UserRepository.Create(new Domain.Entities.User()
            {
                UserName = dto.UserName,
                Password = dto.Password
            });

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> InsertRefreshToken(string userName, string refreshToken, DateTime expiration)
        {
            var user = await GetUserByName(userName);
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = expiration;
            await _unitOfWork.UserRepository.Update(user);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
        public async Task<Domain.Entities.User> GetUserByName(string userName)
        {
            var user = await _unitOfWork.UserRepository.GetByUserName(userName);
            if (user == null)
                throw new Exception($"{userName} username bulunamadı.");

            return user;
        }
    }
}
