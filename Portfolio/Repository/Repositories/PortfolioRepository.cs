using Abstraction.Interfaces.Repositories;
using Abstraction.Models;
using Microsoft.EntityFrameworkCore;
using Repository;
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
