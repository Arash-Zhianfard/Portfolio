using Abstraction.Models;

namespace Abstraction.Interfaces.Services
{
    public interface IAuthService
    {
        TokenInfo GetTokenInfo(string authToken);
        Task<JwtInfo?> SignIn(string username, string password);
        bool ValidateToken(string authToken);
        Task<JwtInfo?> SignUp(SignupRequest signupRequest);
    }
}
