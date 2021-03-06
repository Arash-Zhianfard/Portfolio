using Abstraction.Models;

namespace Abstraction.Interfaces.Repositories
{
    public interface IStockRepository : IBaseRepository<Stock>
    {
        Task<Stock?> GetAsync(string symbol, int userId, int portfolioId);
        Task DeleteRange(List<Stock> stock);
    }
}