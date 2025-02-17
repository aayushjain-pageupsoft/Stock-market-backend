using Backend.Models;
using Backend.Dtos.Comment;
using Backend.Helpers;

namespace Backend.Interfaces
{
    public interface ICommentRepository
    {
        /// <summary>
        /// Get all comments
        /// </summary>
        /// <param name="queryObject"></param>
        /// <returns></returns>
        Task<List<Comment>> GetAllAsync(CommentQueryObject queryObject);
        /// <summary>
        /// Get comment by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Comment?> GetByIdAsync(int id);
        /// <summary>
        /// Create a new comment
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task<Comment> CreateAsync(Comment comment);
        /// <summary>
        ///     Update a comment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task<Comment?> UpdateAsync(int id, Comment comment);

        /// <summary>
        ///    Delete a comment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Comment?> DeleteAsync(int id);
    }
}
