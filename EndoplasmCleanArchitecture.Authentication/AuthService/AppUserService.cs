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

namespace EndoplasmCleanArchitecture.Authentication.AuthService
{
    public class AppUserService : IAppUserService
    {

        private readonly IUserService _userService;
        public AppUserService(IUserService userService)
        {
            _userService = userService;
        }
        //private List<User> _users = new List<User>() {
        //    new User{UserName ="admin",Password="admin"}
        //};

        public async Task<AppUser> Authenticate(string username, string password)
        {
            var user = await _userService.GetUserByName(username);
            if (user == null)
                return new AppUser();

            return await Task.FromResult(new AppUser(user));
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
