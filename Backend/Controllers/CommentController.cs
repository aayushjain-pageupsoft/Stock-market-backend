using Backend.Dtos.Comment;
using Backend.Interfaces;
using Backend.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        // Instantiate the ICommentRepository and IStockRepository interfaces
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;
        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
        }

        /// <summary>
        /// Get a list of all the comments
        /// </summary>
        /// <returns>List of all the comment objects</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if(!ModelState.IsValid) return BadRequest(ModelState); // Check if the model state is valid
            var comments = await _commentRepository.GetAllAsync();
            var commentDtos = comments.Select(c => c.ToCommentDto());
            return Ok(commentDtos);
        }

        [HttpGet]
        [Route("{id:int}")]
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
        /// <param name="id"></param>
        /// <param name="commentDto"></param>
        /// <returns> returns the newly created item object</returns>
        [HttpPost]
        [Route("{id:int}")]
        public async Task<IActionResult> Create([FromRoute] int id, [FromBody] CreateCommentDto commentDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); // Check if the model state is valid
            if (!await _stockRepository.StockExist(id))
            {
                return BadRequest("Stock does not exist!");
            }
            var commentModel = commentDto.ToCommentFromCreate(id);
            await _commentRepository.CreateAsync(commentModel);
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
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto commentDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); // Check if the model state is valid
            var comment = await _commentRepository.UpdateAsync(id, commentDto.ToCommentFromUpdate());
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
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); // Check if the model state is valid
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound("Comment Not found");
            }
            await _commentRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
