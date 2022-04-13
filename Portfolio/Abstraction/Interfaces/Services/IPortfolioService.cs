using Abstraction.Models;

namespace Abstraction.Interfaces.Services
{
    public interface IPortfolioService
    {
        Task<Portfolio> AddAsync(Portfolio entity);
        Task<ICollection<Portfolio>> GetByUserId(int userId);
    
        Task<ICollection<PortfolioItem>> Get(int protfolioId);
    }
}
