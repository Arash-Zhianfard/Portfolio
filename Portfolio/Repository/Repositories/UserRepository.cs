using Abstraction.Interfaces.Repositories;
using Abstraction.Models;
using Repository.BaseRepository;

namespace Repository.Repositories
{

    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext db) : base(db)
        {

        }
    }
}
