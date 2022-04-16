using Abstraction.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class TransactionRequest
    {
        public int PortfolioId { get; set; }
        public string Symbol { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int  Contract { get; set; }
      
        public int CurrencyId
        {
            get;
            set;
        }
        
        [Required]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid price")]
        public string Price { get; set; }
        public IEnumerable<SelectListItem>? Currencies { get; set; }
        public TransactionType Type { get; set; }
    }

}
