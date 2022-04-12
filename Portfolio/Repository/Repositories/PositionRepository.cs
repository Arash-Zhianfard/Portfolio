using Abstraction.Interfaces.Repositories;
using Abstraction.Models;
using Repository.BaseRepository;

namespace Repository.Repositories
{

    public class PositionRepository : BaseRepository<Position>, IPositionRepository
    {
        public PositionRepository(AppDbContext db) : base(db)
        {

        }
    }
}
