using System.Runtime.CompilerServices;
using Backend.Dtos.Stock;
using Backend.Models;
using Backend.Mappers;

namespace Backend.Mappers
{
    public static class StockMappers
    {
        /// <summary>
        /// Maps a Stock object to a StockDto object
        /// </summary>
        /// <param name="stock"></param>
        /// <returns></returns>
        public static StockDto ToStockDto(this Stock stock)
        {
            return new StockDto
            {
                Id = stock.Id,
                Symbol = stock.Symbol,
                CompanyName = stock.CompanyName,
                Purchase = stock.Purchase,
                LastDiv = stock.LastDiv,
                Industry = stock.Industry,
                MarketCap = stock.MarketCap,
                Comments = stock.Comments.Select(c => c.ToCommentDto()).ToList()
            };
        }
        /// <summary>
        ///     Maps a CreateStockRequestDto object to a Stock object
        /// </summary>
        /// <param name="stockDto"></param>
        /// <returns></returns>
        public static Stock ToStockFromCreateDTO(this CreateStockRequestDto stockDto)
        {
            return new Stock
            {
                Symbol = stockDto.Symbol,
                CompanyName = stockDto.CompanyName,
                Purchase = stockDto.Purchase,
                LastDiv = stockDto.LastDiv,
                Industry = stockDto.Industry,
                MarketCap = stockDto.MarketCap
            };
        }
        /// <summary>
        ///    Maps a UpdateStockRequestDto object to a Stock object
        /// </summary>
        /// <param name="fmpStock"></param>
        /// <returns></returns>
        public static Stock ToStockFromFMP(this FMPStock fmpStock)
        {
            return new Stock
            {
                Symbol = fmpStock.symbol,
                CompanyName = fmpStock.companyName,
                Purchase = (decimal)fmpStock.price,
                LastDiv = (decimal)fmpStock.lastDiv,
                Industry = fmpStock.industry,
                MarketCap = fmpStock.mktCap
            };
        }
    }
}
