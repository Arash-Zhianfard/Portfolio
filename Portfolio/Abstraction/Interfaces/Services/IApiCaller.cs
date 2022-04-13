using Service.Model;

namespace Abstraction.Interfaces.Services
{
    public interface IApiCaller
    {
        Task<T> GetAsync<T>(RequestOption requestOption);

    }
}
