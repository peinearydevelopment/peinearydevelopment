using System.Collections.Generic;

namespace DataAccess.Contracts.Blog
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string WebsiteUrl { get; set; }
        public string MarkdownText { get; set; }
        public ICollection<PostCommentDto> Posts { get; set; }
    }
}
