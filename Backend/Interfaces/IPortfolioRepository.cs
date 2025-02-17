using Backend.Models;

namespace Backend.Interfaces
{
    public interface IPortfolioRepository
    {
        /// <summary>
        /// Get the portfolio of a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<List<Stock>> GetUserPortfolio(AppUser user);
        /// <summary>
        ///     Create a new portfolio
        /// </summary>
        /// <param name="portfolio"></param>
        /// <returns></returns>
        Task<Portfolio> CreateAsync(Portfolio portfolio);
        /// <summary>
        ///    Delete a portfolio
        /// </summary>
        /// <param name="appUser"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        Task<Portfolio> DeletePortfolio(AppUser appUser, string symbol);
    }
}
