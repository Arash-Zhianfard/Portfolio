using Abstraction.Models;

namespace Abstraction.Interfaces.Services
{
    public interface IPortfolioService
    {
        Task<Portfolio> AddAsync(Portfolio entity);    
    }
}
