using Abstraction.Interfaces.Repositories;
using Abstraction.Models;
using Repository.BaseRepository;

namespace Repository.Repositories
{

    public class WatchListRepository : BaseRepository<WatchList>, IWatchListRepository
    {
        public WatchListRepository(AppDbContext db) : base(db)
        {

        }
    }
}
