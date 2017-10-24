using System;

namespace DataAccess.Contracts.Blog
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string WebsiteUrl { get; set; }
        public string MarkdownText { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? LastUpdatedOn { get; set; }
        public DateTimeOffset? ApprovedOn { get; set; }
        public string Uid { get; set; }
        public int PostId { get; set; }
        public int? CommentRespondedToId { get; set; }

        public virtual PostDto Post { get; set; }
        public virtual CommentDto CommentRespondedTo { get; set; }
    }
}
