using Abstraction.Interfaces.Services;
using Abstraction.Models;

namespace Abstraction.Interfaces.Repositories
{
    public interface IPortfolioRepository : IBaseRepository<Portfolio>
    {
        Task<ICollection<Portfolio>> GetByUserId(int userId);
        Task<Portfolio?> GetPortfolioItems(int portfolioId);
    }
}
