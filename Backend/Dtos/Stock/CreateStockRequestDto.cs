using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.Dtos.Stock
{
    public class CreateStockRequestDto
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Symbol cannot be over 10 characters")]
        public string Symbol { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(20, ErrorMessage = "Company name cannot be over 20 characters")]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [Range(1,1000000000, ErrorMessage = "Purchase must be in range 1 - 1000000000")]
        public decimal Purchase { get; set; }

        [Required]
        [Range(0.001, 100, ErrorMessage = "Purchase must be in range 0.001 - 100")]
        public decimal LastDiv { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "Industry cannot be over 10 characters")]
        public string Industry { get; set; } = string.Empty;

        [Required]
        [Range(1, 5000000000, ErrorMessage = "Market cap must be in range 1 - 5000000000")]
        public long MarketCap { get; set; }
    }
}
