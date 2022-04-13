using Abstraction.Models;

namespace Abstraction.Interfaces.Services
{
    public interface IWatchListService
    {
        Task<WatchList> AddAsync(WatchList watchlist);
    }
}
