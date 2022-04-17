using Abstraction.Models;

namespace Abstraction.Interfaces.Services
{
    public interface IExchangeService
    {
        Task<Position> RemovePosition(PositionRequest positionRequest);
        Task<Position> AddPosition(PositionRequest positionRequest);
    }
}
