using System;

namespace Contracts.Blog
{
    public class Post : IPostable
    {
        public string Title { get; set; }
        public string MarkdownContent { get; set; }
        public string HTMLContent { get; set; }
        public Tag[] Tags { get; set; }
        public DateTimeOffset PostedOn { get; set; }
        public int Views { get; set; }
        public Comment[] Comments { get; set; }

        public NavigationPost NextPost { get; set; }
        public NavigationPost PreviousPost { get; set; }
        public object[] RelatedPosts { get; set; } // title, slug, display order
    }
}
