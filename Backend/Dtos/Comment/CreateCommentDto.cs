﻿using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.Comment
{
    public class CreateCommentDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title must be at least 5 characters long")]
        [MaxLength(100, ErrorMessage = "Title cannot be over 100 characters")]

        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(5, ErrorMessage = "Content must be at least 5 characters long")]
        [MaxLength(400, ErrorMessage = "Content cannot be over 400 characters")]
        public string Content { get; set; } = string.Empty;  
    }
}
