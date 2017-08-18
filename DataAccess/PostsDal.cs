using System.Threading.Tasks;
using DataAccess.Contracts;
using DataAccess.Contracts.Blog;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccess
{
    public class PostsDal : IPostsDal
    {
        private PdDbContext DbContext { get; }

        public PostsDal(PdDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<ResultSetDto<PostDto>> Search(int pageIndex, int pageSize)
        {
            var resultsTask = DbContext.Posts
                                       .AsNoTracking()
                                       .OrderByDescending(p => p.PostedOn)
                                       .Skip(pageIndex * pageSize)
                                       .Take(pageSize)
                                       .ToArrayAsync();

            var postsCountTask = DbContext.Posts
                                          .AsNoTracking()
                                          .CountAsync();

            await Task.WhenAll(resultsTask, postsCountTask).ConfigureAwait(false);

            return new ResultSetDto<PostDto>
            {
                Results = resultsTask.Result,
                TotalResults = postsCountTask.Result
            };
        }

        public async Task<ResultSetDto<PostDto>> Search(int pageIndex, int pageSize, string searchTerm)
        {
            var resultsTask = DbContext.Posts
                                       .AsNoTracking()
                                       .Where(p => p.Title.Contains(searchTerm) || p.MarkdownContent.Contains(searchTerm))
                                       .OrderByDescending(p => p.PostedOn)
                                       .Skip(pageIndex * pageSize)
                                       .Take(pageSize)
                                       .ToArrayAsync();

            var postsCountTask = DbContext.Posts
                                          .AsNoTracking()
                                          .CountAsync();

            await Task.WhenAll(resultsTask, postsCountTask).ConfigureAwait(false);

            return new ResultSetDto<PostDto>
            {
                Results = resultsTask.Result,
                TotalResults = postsCountTask.Result
            };
        }
    }
}
