using System.Threading.Tasks;
using DataAccess.Contracts;
using DataAccess.Contracts.Blog;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System;

namespace DataAccess
{
    public class PostsDal : IPostsDal
    {
        private PdDbContext DbContext { get; }

        public PostsDal(PdDbContext dbContext) => DbContext = dbContext;

        public Task<PostDto> Read(Expression<Func<PostDto, bool>> predicate)
        {
            return DbContext.Posts
                            .AsNoTracking()
                            .Include(p => p.Tags)
                                .ThenInclude(pt => pt.Tag)
                            .Include(p => p.Comments)
                                .ThenInclude(pc => pc.Comment)
                            .FirstOrDefaultAsync(predicate);
        }

        public Task<PostDto[]> ReadMany(Expression<Func<PostDto, bool>> predicate)
        {
            return DbContext.Posts
                            .AsNoTracking()
                            .Include(p => p.Tags)
                                .ThenInclude(pt => pt.Tag)
                            .Include(p => p.Comments)
                                .ThenInclude(pc => pc.Comment)
                            .Where(predicate)
                            .ToArrayAsync();
        }

        public Task<PostDto> ReadPrevious(DateTimeOffset postedOn)
        {
            return DbContext.Posts
                            .Where(p => p.PostedOn != null)
                            .Where(p => p.PostedOn < postedOn)
                            .OrderByDescending(p => p.PostedOn)
                            .FirstOrDefaultAsync();
        }

        public Task<PostDto> ReadNext(DateTimeOffset postedOn)
        {
            return DbContext.Posts
                            .Where(p => p.PostedOn != null)
                            .Where(p => p.PostedOn > postedOn)
                            .OrderBy(p => p.PostedOn)
                            .FirstOrDefaultAsync();
        }

        public async Task<ResultSetDto<PostDto>> Search(int pageIndex, int pageSize)
        {
            var resultsTask = DbContext.Posts
                                       .AsNoTracking()
                                       .Include(p => p.Tags)
                                            .ThenInclude(pt => pt.Tag)
                                       .Where(p => p.PostedOn != null)
                                       .OrderByDescending(p => p.PostedOn)
                                       .Skip(pageIndex * pageSize)
                                       .Take(pageSize)
                                       .ToArrayAsync();

            var postsCountTask = DbContext.Posts
                                          .AsNoTracking()
                                          .Where(p => p.PostedOn != null)
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
                                       .Include(p => p.Tags)
                                            .ThenInclude(pt => pt.Tag)
                                       .Where(p => p.Title.Contains(searchTerm) || p.MarkdownContent.Contains(searchTerm))
                                       .Where(p => p.PostedOn != null)
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
