using Abstraction.Interfaces.Services;
using Abstraction.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Implementation;
using WebApp.Filter;
using WebApp.Models;

namespace WebApp.Controllers
{
    [AuthorizeFilterAttribute]
    public class HomeController : BasePortfolioController
    {
        private readonly IPortfolioService _portfolioService;
        public HomeController(IAuthService authService, IPortfolioService portfolioService) : base(authService)
        {
            _portfolioService = portfolioService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _portfolioService.GetByUserIdAsync(UserId);
            return View(result);
        }
        public async Task<IActionResult> CreateIndex()
        {
            return View();
        }
        public async Task<IActionResult> Create(PortfolioRequest portfolioRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ErrorMessage = ModelState.ToErrorMessage();
                    return View("CreateIndex");
                }
                var result = await _portfolioService.AddAsync(new Portfolio()
                {
                    Name = portfolioRequest.Name,
                    UserId = UserId
                });
                return RedirectToAction("index");
            }
            catch (Exception ex)
            {
                //exception should be log here
                ViewBag.ErrorMessage = ex.GetType() == typeof(CustomException) ? ex.Message : "something went wrong";
                return View("CreateIndex");
            }
        }
    }
}