using Abstraction.Models;

namespace Abstraction.Interfaces.Services
{
    public interface IVwdService
    {
        Task<List<VwdResponse>> GetAsync(List<string> vwdKeys);
        Task<VwdResponse> GetAsync(string vwdKey);
    }
}
