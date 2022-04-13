using Abstraction.Interfaces.Repositories;
using Abstraction.Interfaces.Services;
using Abstraction.Models;

namespace Service.Implementation
{
    public class StockService : IStockService
    {
        private readonly IStockRepository stockRepository;

        public StockService(IStockRepository stockRepository)
        {
            this.stockRepository = stockRepository;
        }

        public Task<Stock> AddAsync(Stock entity)
        {
            return this.stockRepository.AddAsync(entity);
        } 
    }
}
