using Abstraction.Interfaces.Services;
using Abstraction.Models;
using Microsoft.Extensions.Options;
using Service.Model;

namespace Service.Implementation
{
    public class CurrencyConvertor : ICurrencyConvertor
    {
        private readonly IApiCaller _apiCaller;
        private readonly ConvertCurrencyApiSetting _convertCurrencyApiSetting;
        public CurrencyConvertor(IApiCaller apiCaller, IOptions<ConvertCurrencyApiSetting> options)
        {
            _apiCaller = apiCaller;
            _convertCurrencyApiSetting = options.Value;
        }
        public async Task<double> Convert(string from, string to)
        {
            var response = new List<KeyValuePair<string, string>>();
            response.Add(new KeyValuePair<string, string>("q", from + '_' + to));
            response.Add(new KeyValuePair<string, string>("compact", "ultra"));
            response.Add(new KeyValuePair<string, string>("apiKey", _convertCurrencyApiSetting.ApiKey));
            var result = await _apiCaller.GetAsync<ConvertCurrencyResponse>(new RequestOption
            {
                Url = _convertCurrencyApiSetting.BaseUrl + _convertCurrencyApiSetting.ConvertRoute,
                QueryStringItems = response,
            });
            return result.Value;
        }
        public List<CurrencyItem> GetList()
        {
            var currencyList = new List<CurrencyItem>();
            currencyList.Add(new CurrencyItem("USD", 1));
            currencyList.Add(new CurrencyItem("TJS", 2));
            currencyList.Add(new CurrencyItem("TTD", 3));
            currencyList.Add(new CurrencyItem("EUR", 4));
            return currencyList;
        }
    }
}
