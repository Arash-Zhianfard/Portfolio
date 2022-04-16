using Abstraction.Interfaces.Services;
using Abstraction.Models;
using Microsoft.Extensions.Options;
using Service.Model;

namespace Service.Implementation
{
    public class CurrencyConvertor : ICurrencyConvertor
    {
        private IApiCaller _ApiCaller;
        private ConvertCurrencyApiSetting _ConvertCurrencyApiSetting;
        public CurrencyConvertor(IApiCaller apiCaller, IOptions<ConvertCurrencyApiSetting> options)
        {
            _ApiCaller = apiCaller;
            _ConvertCurrencyApiSetting= options.Value;
        }
        public async Task<double> Convert(string from, string to)
        {
            var response = new List<KeyValuePair<string, string>>();
            response.Add(new KeyValuePair<string, string>("q", from + '_' + to));
            response.Add(new KeyValuePair<string, string>("compact", "ultra"));
            
            response.Add(new KeyValuePair<string, string>("apiKey", _ConvertCurrencyApiSetting.ApiKey));
            var result = await _ApiCaller.GetAsync<ConvertCurrencyResponse>(new RequestOption
            {
                Url = _ConvertCurrencyApiSetting.BaseUrl + "/convert",
                QueryStringItems = response,
            });
            return result.Value;
        }
        public async Task<List<CurrencyItem>> GetListAsync()
        {
            var resi = new List<CurrencyItem>();
            resi.Add(new CurrencyItem("USD", 1));
            resi.Add(new CurrencyItem("TJS", 2));
            resi.Add(new CurrencyItem("TTD", 3));
            resi.Add(new CurrencyItem("EUR", 4));
            return resi;
        }
    }
}
