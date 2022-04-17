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

        public async Task<Stock?> GetAsync(string symbol, int userId, int portfolioId)
        {
            var query = from user in _appDbContext.Users
                        join port in _appDbContext.Portfolios
                            on user.Id equals port.UserId
                        join posi in _appDbContext.Positions
                            on port.Id equals posi.PortfolioId
                        join stck in _appDbContext.Stocks
                            on posi.StockId equals stck.Id
                        where user.Id == userId && stck.Symbol.ToLower() == symbol.ToLower() && port.Id == portfolioId
                        select new Position()
                        {

                            Contract = posi.Contract,
                            StockId = posi.StockId,
                            PortfolioId = posi.PortfolioId,
                            Bought = posi.Bought,
                            TransactionType = posi.TransactionType,
                            Stock = stck,
                            Price = posi.Price
                        };
            var result = query.GroupBy(x => x.Stock.Symbol).Select(x => new Stock()
            {
                Id = x.First().Stock.Id,
                Isin = x.First().Stock.Isin,
                Name = x.First().Stock.Name,
                Symbol = x.First().Stock.Symbol,
                Positions = x.ToList()
            }).FirstOrDefault();
            return result;
        }

   

        public async Task DeleteRange(List<Stock> stock)
        {
            _appDbContext.Stocks.RemoveRange(stock);
            await _appDbContext.SaveChangesAsync();
        }


    }
}
