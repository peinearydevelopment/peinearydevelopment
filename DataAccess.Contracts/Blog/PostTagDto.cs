namespace DataAccess.Contracts.Blog
{
    public class PostTagDto
    {
        public int PostId { get; set; }
        public PostDto Post { get; set; }
        public int TagId { get; set; }
        public TagDto Tag { get; set; }
    }
}
