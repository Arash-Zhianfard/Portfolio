using Abstraction.Interfaces.Repositories;
using Abstraction.Interfaces.Services;
using Abstraction.Models;

namespace Service.Implementation
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;

        public StockService(IStockRepository stockRepository)
        {
            this._stockRepository = stockRepository;
        }

        public Task<Stock?> GetAsync(int id)
        {
            return _stockRepository.GetAsync(id);
        }

        public Task<Stock> AddAsync(Stock entity)
        {
            return this._stockRepository.AddAsync(entity);
        }

        public async Task DeleteAsync(Stock stock)
        {
            await _stockRepository.DeleteAsync(stock);
        }

        public Task<Stock?> GetAsync(string symbol, int userId,int portfolioId)
        {
            return this._stockRepository.GetAsync(symbol, userId, portfolioId);
        }
    }
}
