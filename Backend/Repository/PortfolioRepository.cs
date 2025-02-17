using Backend.Data;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContext _dbContext;
        public PortfolioRepository(ApplicationDBContext context)
        {
            _dbContext = context;
        }
        /// <summary>
        /// Create portfolio
        /// </summary>
        /// <param name="portfolio"></param>
        /// <returns></returns>
        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _dbContext.Portfolios.AddAsync(portfolio);
            await _dbContext.SaveChangesAsync();
            return portfolio;
        }
        /// <summary>
        /// Delete portfolio
        /// </summary>
        /// <param name="appUser"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public async Task<Portfolio> DeletePortfolio(AppUser appUser, string symbol)
        {
            // Find the portfolio wheere the appUser id and stock symbol match
            var portfolioModel = await _dbContext.Portfolios.FirstOrDefaultAsync(x => x.AppUserId == appUser.Id && x.Stock.Symbol.ToLower() == symbol.ToLower());

            if (portfolioModel == null)
            {
                return null;
            }

            _dbContext.Portfolios.Remove(portfolioModel);
            await _dbContext.SaveChangesAsync();
            return portfolioModel;
        }
        /// <summary>
        /// Get the portfolio of a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            // Get the portfolio of a user
            return await _dbContext.Portfolios.Where(u => u.AppUserId == user.Id).Select(stock => new Stock // Select the stock properties
            {
                Id = stock.StockId,
                Symbol = stock.Stock.Symbol,
                CompanyName = stock.Stock.CompanyName,
                Purchase = stock.Stock.Purchase,
                LastDiv = stock.Stock.LastDiv,
                Industry = stock.Stock.Industry,
                MarketCap = stock.Stock.MarketCap,
            }).ToListAsync();
        }
    }
}
