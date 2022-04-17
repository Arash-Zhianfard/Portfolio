using Abstraction.Models;

namespace Abstraction.Interfaces.Services
{
    public interface IStockService
    {
        Task<Stock?> GetAsync(string symbol,int userId,int portfolioId);
        Task<Stock?> GetAsync( int id);
        Task<Stock> AddAsync(Stock stock);
        Task DeleteAsync(Stock stock);
    }
}
