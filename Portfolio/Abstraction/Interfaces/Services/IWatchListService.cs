using Abstraction.Models;

namespace Abstraction.Interfaces.Services
{
    public interface IWatchListService
    {
        Task AddAsync(WatchList watchlist);
    }
}
