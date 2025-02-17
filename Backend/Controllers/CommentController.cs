using Backend.Dtos.Comment;
using Backend.Interfaces;
using Backend.Mappers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Backend.Extensions;
using Backend.Models;
using Backend.Helpers;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        // Instantiate the ICommentRepository and IStockRepository interfaces
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IFMPService _fmpService;
        private readonly UserManager<AppUser> _userManager;
        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository, UserManager<AppUser> userManager,
        IFMPService fmpService)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
            _userManager = userManager;
            _fmpService = fmpService;
        }

        /// <summary>
        /// Get a list of all the comments
        /// </summary>
        /// <returns>List of all the comment objects</returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] CommentQueryObject queryObject)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState); // Check if the model state is valid
            var comments = await _commentRepository.GetAllAsync(queryObject);
            var commentDtos = comments.Select(c => c.ToCommentDto());
            return Ok(commentDtos);
        }

        [HttpGet]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); // Check if the model state is valid
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound("Comment Not found");
            }
            return Ok(comment.ToCommentDto());
        }

        /// <summary>
        /// Create a new comment
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="commentDto"></param>
        /// <returns> returns the newly created item object</returns>
        [HttpPost]
        [Route("{symbol:alpha}")]
        [Authorize]
        public async Task<IActionResult> Create([FromRoute] string symbol, [FromBody] CreateCommentDto commentDto)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // Check if the stock exists by the Symbol
            var stock = await _stockRepository.GetBySymbolAsync(symbol);

            if (stock == null)
            {
                // If the stock does not exist, find the stock by the symbol
                stock = await _fmpService.FindStockBySymbolAsync(symbol);
                if (stock == null)
                {
                    return BadRequest("Stock does not exists");
                }
                else
                {
                    // Create the stock if it does not exist
                    await _stockRepository.CreateAsync(stock);
                }
            }
            // Get the username
            var username = User.GetUsername();
            // Find the user by the username
            var appUser = await _userManager.FindByNameAsync(username);

            var commentModel = commentDto.ToCommentFromCreate(stock.Id); // Map the commentDto to the commentModel
            commentModel.AppUserId = appUser.Id; // Set the AppUserId to the user id
            await _commentRepository.CreateAsync(commentModel);  // Create the comment
            // Return the newly created comment object
            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
        }

        /// <summary>
        /// Update a comment by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="commentDto"></param>
        /// <returns> returns the newly updated commetn object</returns>
        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto commentDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); // Check if the model state is valid
            var comment = await _commentRepository.UpdateAsync(id, commentDto.ToCommentFromUpdate(id)); // Update the comment and convert the commentDto to the commentModel
            if (comment == null)
            {
                return NotFound("Comment Not found");
            }
            return Ok(comment.ToCommentDto());
        }

        /// <summary>
        /// Delete a comment by id
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); // Check if the model state is valid
            var comment = await _commentRepository.GetByIdAsync(id); // Get the comment by id
            if (comment == null)
            {
                return NotFound("Comment Not found");
            }
            await _commentRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
