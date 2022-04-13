using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraction.Interfaces.Services;

public class PortfolioItem
{
    public string Symbol { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public double Bought { get; set; }
    public Double Current { get; set; }
    public double Yield { get; set; }
    public override bool Equals(object obj)
    {
        var other = obj as PortfolioItem;
        if (other.Symbol == Symbol
            &&
            other.Name == Name
            &&
            other.Price == Price
             &&
            other.Quantity == Quantity
             &&
            other.Bought == Bought
            &&
            other.Current == Current
            &&
            other.Yield == Yield) 
            return true;
        return false;
    }
}
