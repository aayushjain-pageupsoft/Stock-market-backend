using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Backend.Data;
using Backend.Dtos.Stock;
using Backend.Mappers;
using Microsoft.EntityFrameworkCore;
using Backend.Interfaces;
using Backend.Helpers;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        /// <summary>
        /// The database context
        /// </summary>
        private readonly ApplicationDBContext _dbcontext;
        private readonly IStockRepository _stockRepo;
        public StockController(ApplicationDBContext dbcontext, IStockRepository stockRepo)
        {
            _dbcontext = dbcontext;
            _stockRepo = stockRepo;
        }

        /// <summary>
        /// Get all stocks
        /// </summary>
        /// <returns>List of Stocks</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); // Check if the model state is valid
            var stocks = await _stockRepo.GetAllAsync(query);
            var stockDto =   stocks.Select(s => s.ToStockDto());
            return Ok(stockDto);
        }

        /// <summary>
        /// Get stock by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns> A specific Stock Object</returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); // Check if the model state is valid
            var stock = await _stockRepo.GetByIdAsync(id);
            if (stock == null)
            {
                return NotFound("Stock Not found");
            }
            return Ok(stock.ToStockDto());
        }

        /// <summary>
        /// Create new Stock Item entry
        /// </summary>
        /// <param name="stockDto"></param>
        /// <returns>the newly created stock item </returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); // Check if the model state is valid
            // convert the incoming stock data to a desired type
            var stockModel = stockDto.ToStockFromCreateDTO();
            await _stockRepo.CreateAsync(stockModel);
            // return newly created stock entry by calling the action
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }


        /// <summary>
        /// Update a stock item
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="data"></param>
        /// <returns> returns the updated stock item value object</returns>
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto data)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); // Check if the model state is valid
            var stock = await _stockRepo.UpdateAsync(id, data);
            if (stock == null)
            {
                return NotFound("Stock Not found");
            }
            return Ok(stock.ToStockDto());
        }

        /// <summary>
        /// Delete a stock item
        /// </summary>
        /// <param name="id"></param>
        /// <returns> </returns>
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); // Check if the model state is valid
            var stock = await _stockRepo.DeleteAsync(id);
            if (stock == null)
            {
                return NotFound("Stock Not found");
            }
            return NoContent();
        }
    }
}
