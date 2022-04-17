using System.Linq;
using Abstraction.Interfaces.Repositories;
using Abstraction.Models;
using Microsoft.EntityFrameworkCore;
using Repository.BaseRepository;

namespace Repository.Repositories
{

    public class PortfolioRepository : BaseRepository<Portfolio>, IPortfolioRepository
    {
        private readonly AppDbContext _appDbContext;

        public PortfolioRepository(AppDbContext db) : base(db)
        {
            this._appDbContext = db;
        }

        public async Task<ICollection<Portfolio>> GetByUserIdAsync(int userId)
        {
            var result = await _appDbContext.Portfolios.Where(x => x.User.Id == userId).ToListAsync();
            return result;
        }

        public async Task<Portfolio?> GetPortfolioItemsAsync(int portfolioId)
        {
            var query = _appDbContext.Portfolios.Include(x => x.Positions).ThenInclude(x => x.Stock);
            var result = await query.FirstOrDefaultAsync(x => Enumerable.Any<Position>(x.Positions, x => x.PortfolioId == portfolioId));
            return result;
        }
    }
}
