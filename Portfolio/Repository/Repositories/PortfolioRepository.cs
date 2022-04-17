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

        public async Task<ICollection<Portfolio>> GetByUserId(int userId)
        {
            var result = await _appDbContext.Portfolios.Where(x => x.User.Id == userId).ToListAsync();
            return result;
        }

        public async Task<Portfolio?> GetPortfolioItems(int portfolioId)
        {
            var result = _appDbContext.Portfolios.Include(x => x.Positions).ThenInclude(x => x.Stock);
            return result.FirstOrDefault(x => Enumerable.Any<Position>(x.Positions, x=>x.PortfolioId==portfolioId));
        }
    }
}
