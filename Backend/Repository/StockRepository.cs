﻿using Backend.Data;
using Backend.Models;
using Backend.Dtos.Stock;
using Backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using Backend.Helpers;

namespace Backend.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _dbcontext;

        public StockRepository(ApplicationDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        /// <summary>
        /// Create a new stock
        /// </summary>
        /// <param name="stock"></param>
        /// <returns></returns>
        public async Task<Stock> CreateAsync(Stock stock)
        {
            await _dbcontext.Stocks.AddAsync(stock);
            await _dbcontext.SaveChangesAsync();
            return stock;
        }
        /// <summary>
        /// Delete a stock
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _dbcontext.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (stockModel == null)
            {
                return null;
            }
            _dbcontext.Stocks.Remove(stockModel);
            await _dbcontext.SaveChangesAsync();
            return stockModel;
        }
        /// <summary>
        /// Get all stocks
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            // Start with the base query
            var stocksQuery = _dbcontext.Stocks
                                 .AsQueryable()
                                 .AsNoTracking(); //to improve performance since the entities are not being modified.

            // Apply filters first
            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                // matches pattern anywhere in the string
                stocksQuery = stocksQuery.Where(x => EF.Functions.Like(x.CompanyName, $"%{query.CompanyName}%"));
            }
            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocksQuery = stocksQuery.Where(x => EF.Functions.Like(x.Symbol, $"%{query.Symbol}%"));
            }
            // Apply sorting
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                stocksQuery = query.SortBy.ToLower() switch
                {
                    "symbol" => query.IsDescending ? stocksQuery.OrderByDescending(x => x.Symbol) : stocksQuery.OrderBy(x => x.Symbol),
                    "companyname" => query.IsDescending ? stocksQuery.OrderByDescending(x => x.CompanyName) : stocksQuery.OrderBy(x => x.CompanyName),
                    "purchase" => query.IsDescending ? stocksQuery.OrderByDescending(x => x.Purchase) : stocksQuery.OrderBy(x => x.Purchase),
                    "lastdiv" => query.IsDescending ? stocksQuery.OrderByDescending(x => x.LastDiv) : stocksQuery.OrderBy(x => x.LastDiv),
                    "industry" => query.IsDescending ? stocksQuery.OrderByDescending(x => x.Industry) : stocksQuery.OrderBy(x => x.Industry),
                    "marketcap" => query.IsDescending ? stocksQuery.OrderByDescending(x => x.MarketCap) : stocksQuery.OrderBy(x => x.MarketCap),
                    _ => stocksQuery
                };
            }

            // Apply pagination
                var skipNumber = (query.PageNumber - 1) * query.PageSize;

            // Include related entities after filtering
            return await stocksQuery
                .Skip(skipNumber)
                .Take(query.PageSize)
                .Include(c => c.Comments)
                .ToListAsync();
        }
        /// <summary>
        ///     get stock by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Stock?> GetByIdAsync(int id)
        {
            // Include related entities
            return await _dbcontext.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(x => x.Id == id);
        }
        /// <summary>
        /// Get stock by symbol
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _dbcontext.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
        }
        /// <summary>
        /// Update a stock
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stockDto"></param>
        /// <returns></returns>
        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            // Find the stock by id
            var stock = await _dbcontext.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (stock == null) // if stock is not found
            {
                return null;
            }
            // Update the stock properties
            stock.Symbol = stockDto.Symbol;
            stock.CompanyName = stockDto.CompanyName;
            stock.Purchase = stockDto.Purchase;
            stock.LastDiv = stockDto.LastDiv;
            stock.Industry = stockDto.Industry;
            stock.MarketCap = stockDto.MarketCap;

            await _dbcontext.SaveChangesAsync();
            return stock;
        }
        /// <summary>
        /// Check if a stock exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> StockExist(int id)
        {
            // Check if the stock exists
            return await _dbcontext.Stocks.AnyAsync(x => x.Id == id);
        }
    }
}
