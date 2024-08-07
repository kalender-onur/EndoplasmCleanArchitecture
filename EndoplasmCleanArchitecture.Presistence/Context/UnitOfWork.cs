using EndoplasmCleanArchitecture.Application.Interfaces;
using EndoplasmCleanArchitecture.Application.Interfaces.User;
using EndoplasmCleanArchitecture.Domain.Common;
using EndoplasmCleanArchitecture.Presistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EndoplasmCleanArchitecture.Presistence.Context
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private IUserRepository _userRepository;

        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_dbContext);

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditableEntities();
            await _dbContext.SaveChangesAsync(cancellationToken);

        }


        private void UpdateAuditableEntities()
        {
            IEnumerable<EntityEntry<BaseEntity>> entries = _dbContext.ChangeTracker.Entries<BaseEntity>();

            foreach (EntityEntry<BaseEntity> entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                    entityEntry.Property(x => x.CreatedAt).CurrentValue = DateTime.UtcNow;
                if (entityEntry.State == EntityState.Modified)
                    entityEntry.Property(x => x.UpdatedAt).CurrentValue = DateTime.UtcNow;
            }


        }


        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }
    }
}
