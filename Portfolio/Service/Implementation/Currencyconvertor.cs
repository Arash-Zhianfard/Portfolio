using Abstraction.Interfaces.Services;
using Abstraction.Models;
using Microsoft.Extensions.Options;
using Service.Model;

namespace Service.Implementation
{
    public class CurrencyConvertor : ICurrencyConvertor
    {
        private IApiCaller _apiCaller;
        private ConvertCurrencyApiSetting _convertCurrencyApiSetting;
        public CurrencyConvertor(IApiCaller apiCaller, IOptions<ConvertCurrencyApiSetting> options)
        {
            _apiCaller = apiCaller;
            _convertCurrencyApiSetting= options.Value;
        }
        public async Task<double> Convert(string from, string to)
        {
            var response = new List<KeyValuePair<string, string>>();
            response.Add(new KeyValuePair<string, string>("q", from + '_' + to));
            response.Add(new KeyValuePair<string, string>("compact", "ultra"));
            
            response.Add(new KeyValuePair<string, string>("apiKey", _convertCurrencyApiSetting.ApiKey));
            var result = await _apiCaller.GetAsync<ConvertCurrencyResponse>(new RequestOption
            {
                Url = _convertCurrencyApiSetting.BaseUrl + "/convert",
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
