using Abstraction.Interfaces.Services;
using Newtonsoft.Json;
using Service.Model;
using System.Net;

namespace Service.Implementation
{
    public class ApiCaller : IApiCaller
    {        
        public async Task<T> GetAsync<T>(RequestOption requestOption)
        {
            ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => true;

            var httpClient = new HttpClient();
            var builder = new UriBuilder(requestOption.Url);
            if (requestOption.HeaderParameters != null)
            {
                foreach (var header in requestOption.HeaderParameters)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            if (requestOption.QueryStringItems != null)
            {
                var queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
                foreach (var item in requestOption.QueryStringItems)
                {
                    queryString.Add(item.Key, item.Value.ToString());
                }
                builder.Query = queryString.ToString();
            }
            if (requestOption.Timeout.HasValue)
                httpClient.Timeout = requestOption.Timeout.Value;

            using (var response = await httpClient.GetAsync(builder.ToString()))
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        DefaultValueHandling = DefaultValueHandling.Ignore
                    });
                    return result;
                }
                catch (Exception ex) 
                {
                    //log 
                    throw;
                }
            }
        }

    }
}
