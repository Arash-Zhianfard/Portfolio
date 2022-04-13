using Abstraction.Models;

namespace Abstraction.Interfaces.Services
{
    public interface IVwdService
    {
        Task<VwdResponse> GetAsync(string[] vwdKey);
    }
}
