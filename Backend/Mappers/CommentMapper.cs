using Backend.Models;
using Backend.Dtos.Comment;

namespace Backend.Mappers
{
    public static class CommentMapper
    {
        /// <summary>
        /// Maps a Comment object to a CommentDto object
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public static CommentDto ToCommentDto(this Comment comment)
        {
            return new CommentDto
            {
                Id = comment.Id,
                Title = comment.Title,
                Content = comment.Content,
                CreatedOn = comment.CreatedOn,
                CreatedBy = comment.AppUser.UserName,
                StockId = comment.StockId
            };
        }
        /// <summary>
        ///     Maps a CreateCommentDto object to a Comment object
        /// </summary>
        /// <param name="commentDto"></param>
        /// <param name="stockId"></param>
        /// <returns></returns>
        public static Comment ToCommentFromCreate(this CreateCommentDto commentDto, int stockId)
        {
            return new Comment
            {
                Title = commentDto.Title,
                Content = commentDto.Content,
                StockId = stockId
            };
        }
        /// <summary>
        ///    Maps a UpdateCommentRequestDto object to a Comment object
        /// </summary>
        /// <param name="commentDto"></param>
        /// <param name="stockId"></param>
        /// <returns></returns>
        public static Comment ToCommentFromUpdate(this UpdateCommentRequestDto commentDto, int stockId)
        {
            return new Comment
            {
                Title = commentDto.Title,
                Content = commentDto.Content,
                StockId = stockId
            };
        }
    }
}
