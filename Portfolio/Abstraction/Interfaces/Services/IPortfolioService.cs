using Abstraction.Models;

namespace Abstraction.Interfaces.Services
{
    public interface IPortfolioService
    {
        Task<Portfolio> AddAsync(Portfolio entity);
        Task<ICollection<Portfolio>> GetByUserIdAsync(int userId);
        Task<ICollection<PortfolioItem>> GetItemsAsync(int portfolioId);
        Task<Portfolio> GetPortfolioAsync(int id);
        Task DeleteAsync(Portfolio portfolio);
    }
}