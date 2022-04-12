using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraction.Models
{
    public class WatchList:BaseModel
    {
        public string Name { get; set; }
        public Stock Stock { get; set; }
        public Portfolio Portfolio { get; set; }
        public int StockId { get; set; }
        public int PortfolioId { get; set; }
    }
}
