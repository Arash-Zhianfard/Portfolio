using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraction.Models
{
    public class PositionRequest
    {
        public string Symbol { get; set; }
        public int PortfolioId { get; set; }
        public int UserId { get; set; }
        public int Contract { get; set; }
        public double BuyPrice { get; set; }
    }
}
