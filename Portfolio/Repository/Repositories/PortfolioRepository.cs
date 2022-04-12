using Abstraction.Interfaces.Repositories;
using Abstraction.Models;
using Repository.BaseRepository;

namespace Repository.Repositories
{

    public class PortfolioRepository : BaseRepository<Portfolio>, IPortfolioRepository
    {
        public PortfolioRepository(AppDbContext db) : base(db)
        {

        }
    }
}
