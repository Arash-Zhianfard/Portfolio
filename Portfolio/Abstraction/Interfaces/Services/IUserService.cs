using Abstraction.Models;

namespace Abstraction.Interfaces.Services
{
    public interface IUserService
    {
        Task<User> AddAsync(User entity);
        public Task<User> GetAsync(string username);
        Task<User> GetAsync(string username, string password);
    }
}
