using Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraction.Interfaces.Services
{
    public interface IExchangeService
    {
        Task<Position> Sell(SellRequest sellRequest);
        Task Buy(BuyRequest buyRequest);
    }
}
