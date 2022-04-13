using Abstraction.Models;

namespace Abstraction.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetAsync(string username);
        Task<User?> GetAsync(string username, string password);
    }
}
