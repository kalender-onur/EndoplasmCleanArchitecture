namespace EndoplasmCleanArchitecture.Application.Interfaces.User
{
    public interface IUserRepository : IRepository<Domain.Entities.User>
    {
        Task<Domain.Entities.User?> GetByUserAndPassword(string userName, string password);
        Task<Domain.Entities.User?> GetByUserName(string userName);
        Task<bool> IsUserExists(string userName);
    }
}
