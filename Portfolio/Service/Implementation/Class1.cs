using Abstraction.Interfaces.Services;
using Abstraction.Models;
using Microsoft.Extensions.Options;

namespace Service.Implementation
{
    public class VwdService : IVwdService
    {
        private IApiCaller _apiCaller;

        public VwdService(IApiCaller apiCaller, IOptions<ChannelApiSetting> options)
        {
            _apiCaller = apiCaller;
        }
        public Task<VwdResponse> GetAsync(string[] vwdKeys)
        {
            var response = new List<KeyValuePair<string, string>>();

            foreach (var vwdKey in vwdKeys)
            {
                response.Add(new KeyValuePair<string, string>("vwdKey", vwdKey));           
            }
            _apiCaller.GetAsync<>(new Model.RequestOption
            {
                Url = "",
                QueryStringItems =response,
                
            });
        }
    }
}
