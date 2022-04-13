using Abstraction.Interfaces.Repositories;
using Abstraction.Interfaces.Services;
using Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation
{
    internal class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }
        public Task<User> AddAsync(User entity)
        {
            return this._userRepository.AddAsync(entity);
        }

        public Task<User> GetAsync(string username, string password)
        {
            return this._userRepository.GetAsync(username,password);
        }

    }
}
