using Abstraction.Interfaces.Repositories;
using Abstraction.Models;
using Repository.BaseRepository;

namespace Repository.Repositories
{

    public class StockRepository : BaseRepository<Stock>, IStockRepository
    {
        public StockRepository(AppDbContext db) : base(db)
        {

        }
    }
}
