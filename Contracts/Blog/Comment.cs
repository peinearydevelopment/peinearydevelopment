using System;

namespace Contracts.Blog
{
    public class Comment
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string WebsiteUrl { get; set; }
        public string MarkdownText { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? LastUpdatedOn { get; set; }
        public DateTimeOffset? ApprovedOn { get; set; }
    }
}
