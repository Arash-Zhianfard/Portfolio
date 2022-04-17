using Abstraction.Interfaces.Repositories;
using Abstraction.Models;
using Microsoft.EntityFrameworkCore;
using Repository.BaseRepository;

namespace Repository.Repositories
{

    public class StockRepository : BaseRepository<Stock>, IStockRepository
    {
        private readonly AppDbContext _appDbContext;

        public StockRepository(AppDbContext db) : base(db)
        {
            this._appDbContext = db;
        }

        public async Task<Stock?> GetAsync(string symbol, int userId)
        {
            var query = from user in _appDbContext.Users
                        join port in _appDbContext.Portfolios
                        on user.Id equals port.UserId
                        join posi in _appDbContext.Positions
                        on port.Id equals posi.PortfolioId
                        join stck in _appDbContext.Stocks
                        on posi.StockId equals stck.Id
                        where user.Id == userId && stck.Symbol.ToLower() == symbol.ToLower()
                        select new Stock()
                        {
                            Id = user.Id,
                            Symbol = stck.Symbol,
                            Isin = stck.Isin,
                            Name = stck.Name,
                            Positions = new List<Position> { posi }
                        };
            var result = await query.FirstOrDefaultAsync();
            return result;
        }
    }
}
