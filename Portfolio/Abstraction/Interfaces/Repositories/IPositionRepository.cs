using Abstraction.Models;

namespace Abstraction.Interfaces.Repositories
{
    public interface IPositionRepository : IBaseRepository<Position>
    {
        Task<Position> GetAsync(string stockSymbol, int protfolioId, int userId);
    }
}
