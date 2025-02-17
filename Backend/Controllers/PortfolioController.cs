using Backend.Data;
using Backend.Extensions;
using Backend.Interfaces;
using Backend.Models;
using Backend.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController: ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepository;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IFMPService _fmpService;
        public PortfolioController( IStockRepository stockRepository, UserManager<AppUser> userManager, PortfolioRepository portfolioRepo, IFMPService fmpService)
        { 
            _stockRepository = stockRepository;
            _userManager = userManager;
            _portfolioRepository = portfolioRepo;
            _fmpService = fmpService;
        }

        /// <summary>
        /// Get the user's portfolio
        /// </summary>
        /// <returns> Returns the Portfolio object of the user</returns>

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername(); // Get the username of the current user
            var appUser = await _userManager.FindByNameAsync(username); // Find the user by the username
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser); // Get the user's portfolio
            return Ok(userPortfolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var username = User.GetUsername(); // Get the username of the current user
            var appUser = await _userManager.FindByNameAsync(username); // Find the user by the username
            var stock = await _stockRepository.GetBySymbolAsync(symbol); // Get the stock by the symbol

            if (stock == null) // If the stock does not exist for the given symbol
            {
                stock = await _fmpService.FindStockBySymbolAsync(symbol); // Find the stock by the symbol using the FMP service
                if (stock == null) // If the stock does not exist
                {
                    return BadRequest("Stock does not exists");
                }
                else
                {
                    // Create the stock
                    await _stockRepository.CreateAsync(stock);
                }
            }

            if (stock == null) return BadRequest("Stock not found");

            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser); // Get the user's portfolio
            // Check if the stock is already in the user's portfolio
            if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower())) return BadRequest("Cannot add same stock to portfolio");

            var portfolioModel = new Portfolio // Create a new portfolio model
            {
                StockId = stock.Id,
                AppUserId = appUser.Id
            };
            // Create the portfolio
            await _portfolioRepository.CreateAsync(portfolioModel);

            if (portfolioModel == null) // If the portfolio model is null
            {
                return StatusCode(500, "Could not create");
            }
            else
            {
                return Created();
            }
        }

        /// <summary>
        /// Delete a stock from the user's portfolio
        /// </summary>
        /// <param name="symbol">Stock Symbol to be deleted from portfolio</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var username = User.GetUsername(); //   Get the username of the current user
            var appUser = await _userManager.FindByNameAsync(username); // Find the user by the username

            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser); // Get the user's portfolio

            var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList(); // Filter the stock by the symbol

            if (filteredStock.Count() == 1) // If the stock is in the user's portfolio
            {
                // Delete the stock from the user's portfolio
                await _portfolioRepository.DeletePortfolio(appUser, symbol);
            }
            else // If the stock is not in the user's portfolio
            {
                return BadRequest("Stock not in your portfolio");
            }

            return Ok();
        }
    }
}
 