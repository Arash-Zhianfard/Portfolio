using Abstraction.Interfaces.Services;
using Abstraction.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Implementation;
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
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ErrorMessage = ModelState.ToErrorMessage();
                    return View("SignInIndex");
                }

                var token = await _authService.SignIn(request.Username, request.Password);

                if (token != null)
                {
                    HttpContext.Session.SetString("Token", token.Token);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ErrorMessage = "username or password is wrong";
                    return View("SignInIndex");
                }
            }
            catch (Exception ex)
            {
                //exception should be log here
                ViewBag.ErrorMessage = ex.GetType() == typeof(CustomException) ? ex.Message : "something went wrong";
                return View("SignInIndex");
            }
        }

        public IActionResult SignInIndex()
        {
            return View();
        }
        public IActionResult SignUpIndex()
        {
            return View();
        }
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ErrorMessage = ModelState.ToErrorMessage();
                    return View("SignUpIndex");
                }

                var user = await _userService.GetAsync(request.Username);
                if (user != null)
                {
                    ViewBag.ErrorMessage = "username already existed";
                    return View("SignUpIndex");
                }
                var newPlayerToken = await _authService.SignUp(
                    new SignupRequest
                    {
                        Username = request.Username,
                        Password = request.Password,
                        Email = request.Email,
                    });
                return RedirectToAction("SignInIndex");
            }
            catch (Exception ex)
            {
                //exception should be log here
                ViewBag.ErrorMessage = ex.GetType() == typeof(CustomException) ? ex.Message : "something went wrong";
                return View("SignUpIndex");
            }
        }
    }
}
