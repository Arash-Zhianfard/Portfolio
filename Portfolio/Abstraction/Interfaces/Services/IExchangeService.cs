using Abstraction.Models;

namespace Abstraction.Interfaces.Services
{
    public interface IExchangeService
    {
        Task<Position> RemovePosition(SellRequest sellRequest);
        Task<Position> AddPosition(PositionRequest positionRequest);
    }
}
