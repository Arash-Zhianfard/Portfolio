using Abstraction.Interfaces.Repositories;
using Abstraction.Models;
using Microsoft.EntityFrameworkCore;
using Repository.BaseRepository;

namespace Repository.Repositories
{

    public class StockRepository : BaseRepository<Stock>, IStockRepository
    {
        private readonly AppDbContext _appDbContext;

        public StockRepository(AppDbContext db) : base(db)
        {
            this._appDbContext = db;
        }
    }
}
