using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraction.Models
{
    public class BuyRequest
    {
        public string Symbol { get; set; }
        public string Contract { get; set; }
        public string Price { get; set; }
    }
    public class SellRequest
    {
        public string Symbol { get; set; }
        public int UserId { get; set; }
        public int Contract { get; set; }
        public double Price { get; set; }
    }
}
