using Abstraction.Interfaces.Services;
using Abstraction.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Implementation;
using WebApp.Filter;
using WebApp.Models;

namespace WebApp.Controllers
{
    [AuthorizeFilterAttribute]
    public class PositionController : BasePortfolioController
    {
        private readonly IExchangeService _exchangeService;
        private readonly ICurrencyConvertor _currencyConvertor;
        private readonly IPositionService _positionService;
        private readonly IStockService _stockService;
        public PositionController(IExchangeService exchangeService, IAuthService authService, ICurrencyConvertor currencyConvertor, IPositionService positionService, IStockService stockService) : base(authService)
        {
            _exchangeService = exchangeService;
            _currencyConvertor = currencyConvertor;
            _positionService = positionService;
            _stockService = stockService;
        }
        public async Task<IActionResult> AddTransactionIndex(int portfolioId)
        {
            var currencyList = await _currencyConvertor.GetListAsync();
            return View(new TransactionRequest { PortfolioId = portfolioId, Currencies = GetSelectListItems(currencyList) }); ;
        }
        public async Task<IActionResult> AddTransaction(TransactionRequest transactionRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ErrorMessage = ModelState.ToErrorMessage();
                    return View("AddTransactionIndex", new TransactionRequest { PortfolioId = transactionRequest.PortfolioId });
                }
                var currencyItems = await _currencyConvertor.GetListAsync();
                var currencyName = currencyItems.FirstOrDefault(x => x.Id == transactionRequest.CurrencyId)?.Name;
                if (transactionRequest.Type == TransactionType.Buy)
                {
                    await _exchangeService.AddPosition(new PositionRequest()
                    {
                        Price = double.Parse(transactionRequest.Price),
                        Contract = transactionRequest.Contract,
                        PortfolioId = transactionRequest.PortfolioId,
                        Symbol = transactionRequest.Symbol,
                        UserId = UserId,
                        CurrencyName = currencyName 
                    });
                }
                else
                {
                    await _exchangeService.RemovePosition(new PositionRequest()
                    {
                        Price = double.Parse(transactionRequest.Price),
                        Contract = transactionRequest.Contract,
                        PortfolioId = transactionRequest.PortfolioId,
                        Symbol = transactionRequest.Symbol,
                        UserId = UserId,
                        CurrencyName = currencyName
                    });
                }
                return RedirectToAction("Index", "portfolio", new { id = transactionRequest.PortfolioId });
            }
            catch (Exception ex)
            {
                //exception should be log here
                ViewBag.ErrorMessage = ex.GetType() == typeof(CustomException) ? ex.Message : "something went wrong";
                var currencyList = await _currencyConvertor.GetListAsync();
                return View("AddTransactionIndex", new TransactionRequest { PortfolioId = transactionRequest.PortfolioId, Currencies = GetSelectListItems(currencyList) })
              ;
            }
        }
        public async Task<IActionResult> Delete(int stockId,int portfolioId)
        {
            var stock = await _stockService.GetAsync(stockId);
            if (stock == null)
                throw new CustomException("no stock found In this portfolio");

            await _stockService.DeleteAsync(stock);

            return RedirectToAction("Index", "portfolio", new { id = portfolioId });
        }
        private IEnumerable<SelectListItem> GetSelectListItems(IEnumerable<CurrencyItem> elements)
        {
            var selectList = new List<SelectListItem>();
            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element.Id.ToString(),
                    Text = element.Name
                });
            }
            return selectList;
        }
    }
}
