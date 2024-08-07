using EndoplasmCleanArchitecture.Application.Interfaces.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndoplasmCleanArchitecture.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
        ValueTask DisposeAsync();
    }
}
