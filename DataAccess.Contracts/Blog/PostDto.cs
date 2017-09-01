using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Contracts.Blog
{
    public class PostDto
    {
        public int Id { get; set; }
        [MaxLength(255)]
        public string Title { get; set; }
        public string MarkdownContent { get; set; }
        public string HTMLContent { get; set; }
        [MaxLength(255)]
        public string Slug { get; set; }
        public ICollection<PostTagDto> Tags { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? LastUpdatedOn { get; set; }
        public DateTimeOffset? PostedOn { get; set; }
        public ICollection<PostCommentDto> Comments { get; set; }
    }
}
