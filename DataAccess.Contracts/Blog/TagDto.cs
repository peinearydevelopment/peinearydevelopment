using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Contracts.Blog
{
    public class TagDto
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<PostTagDto> Posts { get; set; }
    }
}
