using EndoplasmCleanArchitecture.Application.Interfaces;
using EndoplasmCleanArchitecture.Domain.Common;
using EndoplasmCleanArchitecture.Presistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndoplasmCleanArchitecture.Presistence.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _dbContext;
        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public async Task Delete(int Id)
        {
            var entity = await GetById(Id);
            _dbContext.Set<T>().Remove(entity);

        }

        public IQueryable<T> GetAll()
        {
            return _dbContext.Set<T>().AsNoTracking();
        }

        public async Task<T> GetById(int id)
        {
            return await _dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }
    }
}
