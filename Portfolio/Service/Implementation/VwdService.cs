﻿using Abstraction.Interfaces.Services;
using Abstraction.Models;
using Microsoft.Extensions.Options;
using Service.Model;

namespace Service.Implementation
{
    public class VwdService : IVwdService
    {
        private readonly IApiCaller _apiCaller;
        private readonly VwdservicesApiSetting _vwdservicesApiSetting;

        public VwdService(IApiCaller apiCaller, IOptions<VwdservicesApiSetting> options)
        {
            _apiCaller = apiCaller;
            _vwdservicesApiSetting = options.Value;
        }
        public async Task<List<VwdResponse>> GetAsync(List<string> vwdKeys)
        {
            var response = new List<KeyValuePair<string, string>>();
            foreach (var vwdKey in vwdKeys)
            {
                response.Add(new KeyValuePair<string, string>("vwdKey", vwdKey));
            }
            var result =await _apiCaller.GetAsync<List<VwdResponse>>(new RequestOption
            {
                Url = _vwdservicesApiSetting.BaseUrl,
                QueryStringItems = response,
            });
            return result;
        }
        public async Task<VwdResponse> GetAsync(string vwdKeys)
        {

            var result =await _apiCaller.GetAsync<VwdResponse>(new RequestOption
            {
                Url = _vwdservicesApiSetting.BaseUrl +"/"+ vwdKeys
            });

            return result;
        }
    }
}
