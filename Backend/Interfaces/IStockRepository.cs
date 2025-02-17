using Backend.Models;
using Backend.Dtos.Stock;
using Backend.Helpers;

namespace Backend.Interfaces
{
    public interface IStockRepository
    {
        /// <summary>
        /// Get all stocks
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<Stock>> GetAllAsync(QueryObject query);
        /// <summary>
        /// Get stock by id3
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Stock?> GetByIdAsync(int id);
        /// <summary>
        /// Get stock by symbol
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        Task<Stock?> GetBySymbolAsync(string symbol);
        /// <summary>
        /// Create a new stock
        /// </summary>
        /// <param name="stock"></param>
        /// <returns></returns>
        Task<Stock> CreateAsync(Stock stock);
        /// <summary>
        /// Update a stock
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stockDto"></param>
        /// <returns></returns>
        Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto);
        /// <summary>
        /// Delete a stock
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Stock?> DeleteAsync(int id);
        /// <summary>
        /// Check if a stock exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> StockExist(int id);
    }
}
