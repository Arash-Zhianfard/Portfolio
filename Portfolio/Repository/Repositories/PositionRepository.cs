using Abstraction.Interfaces.Repositories;
using Abstraction.Models;
using Microsoft.EntityFrameworkCore;
using Repository.BaseRepository;

namespace Repository.Repositories
{

    public class PositionRepository : BaseRepository<Position>, IPositionRepository
    {
        private readonly AppDbContext _appDbContext;

        public PositionRepository(AppDbContext db) : base(db)
        {
            this._appDbContext = db;
        }
        public async Task<Position> GetAsync(string stockSymbol, int protfolioId, int userId)
        {
            var query = from user in _appDbContext.Users
                        join port in _appDbContext.Portfolios
                        on user.Id equals port.UserId
                        join posi in _appDbContext.Positions
                        on port.Id equals posi.PortfolioId
                        join stck in _appDbContext.Stocks
                        on posi.StockId equals stck.Id
                        where user.Id == userId && stck.Symbol == stockSymbol && port.Id == protfolioId
                        select posi;
            var result = await query.FirstOrDefaultAsync();
            return result;
        }
    }
}
