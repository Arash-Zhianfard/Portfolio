using Abstraction.Models;

namespace Abstraction.Interfaces.Services
{
    public interface IStockService
    {
        Task<Stock> GetAsync(string symbol,int userId);
        Task<Stock> AddAsync(Stock stock);
    }
}
