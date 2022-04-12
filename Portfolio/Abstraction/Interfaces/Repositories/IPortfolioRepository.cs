using Abstraction.Models;

namespace Abstraction.Interfaces.Repositories
{
    public interface IPortfolioRepository : IBaseRepository<Portfolio>
    {
        Task<Portfolio> AddAsync(Portfolio portfolio); 
    }
}
