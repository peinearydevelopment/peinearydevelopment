using System;
using System.ComponentModel;

namespace Contracts.Blog
{
    public class Comment
    {
        public int Id { get; set; }
        [DisplayName("Display Name")]
        public string DisplayName { get; set; }
        public string Email { get; set; }
        [DisplayName("Personal Website Url")]
        public string WebsiteUrl { get; set; }
        [DisplayName("Comments")]
        public string MarkdownText { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? LastUpdatedOn { get; set; }
        public DateTimeOffset? ApprovedOn { get; set; }
        public int PostId { get; set; }
    }
}
