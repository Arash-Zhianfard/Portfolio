using Abstraction.Interfaces.Services;
using Abstraction.Models;

namespace Service.Implementation
{
    internal class WatchListService: IWatchListService
    {
        private readonly IWatchListService watchListService;

        public WatchListService(IWatchListService watchListService)
        {
            this.watchListService = watchListService;
        }

        public Task<WatchList> AddAsync(WatchList watchlist)
        {
            return watchListService.AddAsync(watchlist);
        }
    }
}
