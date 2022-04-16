using Abstraction.Models;

namespace Abstraction.Interfaces.Services
{
    public interface IPositionService
    {
        Task<Position> AddAsync(Position entity);
        Task<Position> UpdateAsync(Position entity);
        Task<Position> GetAsync(string stockSymbol,int protfolioId, int userId);
    }
}
