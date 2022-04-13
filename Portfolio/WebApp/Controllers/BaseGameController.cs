using Abstraction.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class BaseGameController : Controller
    {
        private readonly IAuthService _authService;
        public BaseGameController(IAuthService authService)
        {
            _authService = authService;
        }
        public int UserId
        {
            get
            {
                string authHeader = Request.Headers["Authorization"].ToString().Substring("Bearer ".Length);
                var toeknInfo = _authService.GetTokenInfo(authHeader);
                return toeknInfo.UserId;
            }
        }
    }
}
