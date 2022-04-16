using Abstraction.Interfaces.Repositories;
using Abstraction.Models;
using Microsoft.EntityFrameworkCore;
using Repository.BaseRepository;

namespace Repository.Repositories
{

    public class PortfolioRepository : BaseRepository<Portfolio>, IPortfolioRepository
    {
        private readonly AppDbContext appDbContext;

        public PortfolioRepository(AppDbContext db) : base(db)
        {
            this.appDbContext = db;
        }

        public async Task<ICollection<Portfolio>> GetByUserId(int userId)
        {
            var result = await appDbContext.Portfolios.Where(x => x.User.Id == userId).ToListAsync();
            return result;
        }

        public async Task<Portfolio> GetPortfolioItems(int portfolioId)
        {
            var result = await appDbContext.Portfolios.Include(x => x.Positions).ThenInclude(x => x.Stock).FirstOrDefaultAsync();
            return result;
        }
    }
}
