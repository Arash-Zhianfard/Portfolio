using Abstraction.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using WebApp.Filter;

namespace WebApp.Controllers
{
    [AuthorizeFilterAttribute]
    public class PortfolioController : BasePortfolioController
    {
        private readonly IPortfolioService _portfolioService;
        public PortfolioController(IPortfolioService portfolioService,IAuthService authService) : base(authService)
        {
            _portfolioService = portfolioService;
        }

        public async Task<IActionResult> Index(int id)
        {
            var result = await _portfolioService.Get(id);
            ViewData["porfolioId"] = id;
            return View(result);
        }
    }
}
