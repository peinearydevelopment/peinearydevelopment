using System;

namespace Contracts.Blog
{
    public class PostSummary : IPostable
    {
        public string Title { get; set; }
        public string ContentSummary { get; set; }
        public string Slug { get; set; }
        public Tag[] Tags { get; set; }
        public DateTimeOffset PostedOn { get; set; }
        public int Views { get; set; }
        public int CommentsCount { get; set; }
    }
}
