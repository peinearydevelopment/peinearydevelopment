namespace DataAccess.Contracts.Blog
{
    public class PostCommentDto
    {
        public int PostId { get; set; }
        public PostDto Post { get; set; }
        public int CommentId { get; set; }
        public CommentDto Comment { get; set; }
    }
}
