using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraction.Models
{
    public class User : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Email { get; set; }
        public ICollection<Portfolio> Portfolios { get; set; }
    }
}
