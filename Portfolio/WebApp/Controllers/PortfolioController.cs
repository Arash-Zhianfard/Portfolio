using Abstraction.Interfaces.Services;
using Abstraction.Models;
using Microsoft.AspNetCore.Mvc;
using WebApp.Filter;

namespace WebApp.Controllers
{
    [AuthorizeFilterAttribute]
    public class PortfolioController : BasePortfolioController
    {
        private readonly IPortfolioService _portfolioService;

        public PortfolioController(IPortfolioService portfolioService, IAuthService authService) : base(authService)
        {
            _portfolioService = portfolioService;
        }

        public async Task<IActionResult> Index(int id)
        {
            var result = await _portfolioService.GetItemsAsync(id);
            ViewData["porfolioId"] = id;
            return View(result);
        }

        
        public async Task<IActionResult> Delete(int id)
        {
            var portfolio = await _portfolioService.GetPortfolioAsync(id);

            if (portfolio == null)
                throw new CustomException("Portfolio not found");

            await _portfolioService.DeleteAsync(portfolio);

            return RedirectToAction("Index", "Home");
        }
    }
}