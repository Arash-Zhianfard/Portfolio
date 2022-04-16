namespace Abstraction.Interfaces.Services
{
    public interface ICurrencyConvertor 
    {
        Task<double> Convert(string from, string to);
        Task<List<CurrencyItem>> GetListAsync();
    }
}
