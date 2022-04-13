using Abstraction.Models;

namespace Abstraction.Interfaces.Services
{
    public interface IStockService
    {
        Task<Stock> AddAsync(Stock entity);
        

    }
}
