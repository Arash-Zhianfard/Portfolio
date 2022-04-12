using Abstraction.Models;

namespace Abstraction.Interfaces.Services
{
    public interface IPositionService
    {
        Task<Portfolio> AddAsync(Portfolio entity);    
    }
}
