using Backend.Models;

namespace Backend.Interfaces
{
    public interface IFMPService
    {
        /// <summary>
        /// Find a stock by symbol
        /// </summary>
        /// <param name="symbol"> symbol of the stock to be searched </param>
        /// <returns> the details object of the STOCK</returns>
        Task<Stock> FindStockBySymbolAsync(string symbol);
    }
}
