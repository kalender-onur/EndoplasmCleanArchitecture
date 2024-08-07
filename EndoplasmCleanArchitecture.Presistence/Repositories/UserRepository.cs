using EndoplasmCleanArchitecture.Application.Interfaces.User;
using EndoplasmCleanArchitecture.Domain.Entities;
using EndoplasmCleanArchitecture.Presistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndoplasmCleanArchitecture.Presistence.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User?> GetByUserAndPassword(string userName, string password)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.UserName == userName && x.Password == password);

        }

        public async Task<User?> GetByUserName(string userName)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.UserName == userName);

        }

        public async Task<bool> IsUserExists(string userName)
        {
            return await GetAll().AnyAsync(x => x.UserName == userName);
        }
    }
}
