using Abstraction.Interfaces.Services;

namespace Service.Implementation
{
    public class DollerConvertorService : ICurrencyConvertorService
    {
        public double Convert(double value)
        {
            throw new NotImplementedException();
        }
    }
    
    public interface Currency
    {
        public double Value { get; set; }
    }
    public class CurrencyConvertorFacotry
    {
        public void Get()
        {
            if (1 == 1) { }
        }


    }

}
