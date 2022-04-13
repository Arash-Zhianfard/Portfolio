using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraction.Models
{
    public class Stock:BaseModel
    {
        public string Isin { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public ICollection<Position> Positions { get; set; }
    }
}
