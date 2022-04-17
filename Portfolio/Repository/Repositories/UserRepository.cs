using Abstraction.Interfaces.Repositories;
using Abstraction.Models;
using Microsoft.EntityFrameworkCore;
using Repository.BaseRepository;

namespace Repository.Repositories
{

    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly AppDbContext _appDbContext;

        public UserRepository(AppDbContext db) : base(db)
        {
            _appDbContext = db;
        }

        public Task<User?> GetAsync(string username)
        {
            return _appDbContext.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == username.ToLower());

        }

        public  Task<User?> GetAsync(string username, string password)
        {
            return _appDbContext.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == username.ToLower() && x.Password == password);
        }
    }
}
