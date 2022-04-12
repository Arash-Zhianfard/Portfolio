using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraction.Models
{
    public class Stock:BaseModel
    {
        public string Name { get; set; }
        public ICollection<Position> Positions { get; set; }
    }
}
