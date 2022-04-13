using Abstraction.Interfaces.Services;
using Abstraction.Models;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SigninRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var token = await _authService.SignIn(request.Username, request.Password);

            if (token != null)
            {
                return Ok(new { accessToken = token });
            }
            else
                return Unauthorized(new { Message = "UnAuthorized" });
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await _userService.GetAsync(request.Username);
            if (user != null)
            {
                return BadRequest("username already existed");
            }
            var newPlayerToken = await _authService.SignUp(
                new SignupRequest
                {
                    Username = request.Username,
                    Password = request.Password
                });
            return Ok(new { accessToken = newPlayerToken });
        }
    }
}
