using EndoplasmCleanArchitecture.Application.Interfaces.User;
using EndoplasmCleanArchitecture.Authentication.Interfaces;
using EndoplasmCleanArchitecture.Authentication.Models;
using EndoplasmCleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EndoplasmCleanArchitecture.Authentication.Services
{
    public class AppUserService : IAppUserService
    {

        private readonly IUserService _userService;
        private readonly IProtectionService _protectionService;

        public AppUserService(IUserService userService, IProtectionService protectionService)
        {
            _userService = userService;
            _protectionService = protectionService;
        }

        public async Task<BaseResult<AppUser>> Authenticate(string username, string password)
        {
            var user = await _userService.GetUserByName(username);
            if (user == null)
            {
                return new BaseResult<AppUser>
                {
                    errorMessage = "User can't found",
                    isSuccess = false
                };
            }
            if (!_protectionService.Unprotect(user?.Password).Equals(password))
            {
                return new BaseResult<AppUser>
                {
                    errorMessage = "Password is incorrect",
                    isSuccess = false
                };
            }

            return new BaseResult<AppUser>
            {
                data = new AppUser(user),
                isSuccess = true
            };

        }
        public async Task<AppUser> GetUserByName(string username)
        {
            var user = await _userService.GetUserByName(username);
            if (user == null)
                return new AppUser();
            return await Task.FromResult(new AppUser(user));
        }
    }
}
