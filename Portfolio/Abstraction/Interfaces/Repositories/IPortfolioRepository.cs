using Abstraction.Models;

namespace Abstraction.Interfaces.Repositories
{
    public interface IPortfolioRepository : IBaseRepository<Portfolio>
    {
        Task<ICollection<Portfolio>> GetByUserIdAsync(int userId);
        Task<Portfolio?> GetPortfolioItemsAsync(int portfolioId);
    }
}