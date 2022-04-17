using Abstraction.Interfaces.Repositories;
using Abstraction.Interfaces.Services;
using Abstraction.Models;

namespace Service.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Task<User> AddAsync(User entity)
        {
            return _userRepository.AddAsync(entity);
        }

        public Task<User?> GetAsync(string username, string password)
        {
            return _userRepository.GetAsync(username,password);
        }

        public Task<User?> GetAsync(string username)
        {
            return _userRepository.GetAsync(username);
        }
    }
}
