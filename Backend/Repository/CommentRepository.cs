using Backend.Data;
using Backend.Models;
using Backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using Backend.Helpers;

namespace Backend.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;

        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Get all comments
        /// </summary>
        /// <param name="queryObject"></param>
        /// <returns></returns>
        public async Task<List<Comment>> GetAllAsync(CommentQueryObject queryObject)
        {
            var comments = _context.Comments.Include(a => a.AppUser).AsQueryable(); //Include the AppUser and make it queryable

            if (!string.IsNullOrWhiteSpace(queryObject.Symbol)) //Check if the symbol is not null or empty
            {
                comments = comments.Where(s => s.Stock.Symbol == queryObject.Symbol); //Filter the comments by the symbol
            };
            if (queryObject.IsDecsending == true)// Check if the comments should be in descending order
            {
                comments = comments.OrderByDescending(c => c.CreatedOn);
            }
            return await comments.ToListAsync();
        }
        /// <summary>
        /// Get comment by id
        /// </summary>
        /// <param name="id"> Comment ID</param>
        /// <returns></returns>
        public async Task<Comment?> GetByIdAsync(int id)
        {
            // Include the AppUser and return the first comment with the id
            return await _context.Comments.Include(a => a.AppUser).FirstOrDefaultAsync(c => c.Id == id);
        }
        /// <summary>
        /// Create a new comment
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public async Task<Comment> CreateAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment); //Add the comment to the database
            await _context.SaveChangesAsync(); //Save the changes
            return comment;
        }
        /// <summary>
        /// Update a comment
        /// </summary>
        /// <param name="id"> Comment id</param>
        /// <param name="comment">Data to be updated</param>
        /// <returns></returns>
        public async Task<Comment?> UpdateAsync(int id, Comment comment)
        {
            // Find the comment with the id
            var commentModel = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if (commentModel == null) { return null; }

            commentModel.Title = comment.Title;
            commentModel.Content = comment.Content;

            await _context.SaveChangesAsync();
            return commentModel;
        }
        /// <summary>
        /// Delete a comment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Comment?> DeleteAsync(int id)
        {
            // Find the comment with the id
            var commentModel = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if (commentModel == null) { return null; }
            _context.Comments.Remove(commentModel); //Remove the comment
            await _context.SaveChangesAsync();
            return commentModel;
        }
    }
}
