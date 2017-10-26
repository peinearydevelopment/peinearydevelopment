using System.Threading.Tasks;
using DataAccess.Contracts;
using DataAccess.Contracts.Blog;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System;
using System.Threading;

namespace DataAccess
{
    public class PostsDal : IPostsDal
    {
        private PdDbContext DbContext { get; }

        public PostsDal(PdDbContext dbContext) => DbContext = dbContext;

        public Task<PostDto> Read(Expression<Func<PostDto, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return DbContext.Posts
                            .AsNoTracking()
                            .Include(p => p.Tags)
                                .ThenInclude(pt => pt.Tag)
                            .Include(p => p.Comments)
                            .FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public Task<PostDto[]> ReadMany(Expression<Func<PostDto, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return DbContext.Posts
                            .AsNoTracking()
                            .Include(p => p.Tags)
                                .ThenInclude(pt => pt.Tag)
                            .Include(p => p.Comments)
                            .Where(predicate)
                            .ToArrayAsync(cancellationToken);
        }

        public Task<PostDto> ReadPrevious(DateTimeOffset postedOn, CancellationToken cancellationToken = default(CancellationToken))
        {
            return DbContext.Posts
                            .Where(p => p.PostedOn != null)
                            .Where(p => p.PostedOn < postedOn)
                            .OrderByDescending(p => p.PostedOn)
                            .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<PostDto> ReadNext(DateTimeOffset postedOn, CancellationToken cancellationToken = default(CancellationToken))
        {
            return DbContext.Posts
                            .Where(p => p.PostedOn != null)
                            .Where(p => p.PostedOn > postedOn)
                            .OrderBy(p => p.PostedOn)
                            .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<ResultSetDto<PostDto>> Search(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            var resultsTask = DbContext.Posts
                                       .AsNoTracking()
                                       .Include(p => p.Tags)
                                            .ThenInclude(pt => pt.Tag)
                                       .Where(p => p.PostedOn != null)
                                       .OrderByDescending(p => p.PostedOn)
                                       .Skip(pageIndex * pageSize)
                                       .Take(pageSize)
                                       .ToArrayAsync(cancellationToken);

            var postsCountTask = DbContext.Posts
                                          .AsNoTracking()
                                          .Where(p => p.PostedOn != null)
                                          .CountAsync(cancellationToken);

            await Task.WhenAll(resultsTask, postsCountTask).ConfigureAwait(false);

            return new ResultSetDto<PostDto>
            {
                Results = resultsTask.Result,
                TotalResults = postsCountTask.Result
            };
        }

        public async Task<ResultSetDto<PostDto>> Search(int pageIndex, int pageSize, string searchTerm, CancellationToken cancellationToken = default(CancellationToken))
        {
            var likeSearchTerm = $"%{searchTerm}%";
            var resultsTask = DbContext.Posts
                                       .AsNoTracking()
                                       .Include(p => p.Tags)
                                            .ThenInclude(pt => pt.Tag)
                                       .Where(p => EF.Functions.Like(p.Title, likeSearchTerm) || EF.Functions.Like(p.MarkdownContent, likeSearchTerm))
                                       .Where(p => p.PostedOn != null)
                                       .OrderByDescending(p => p.PostedOn)
                                       .Skip(pageIndex * pageSize)
                                       .Take(pageSize)
                                       .ToArrayAsync(cancellationToken);

            var postsCountTask = DbContext.Posts
                                          .AsNoTracking()
                                          .Where(p => EF.Functions.Like(p.Title, likeSearchTerm) || EF.Functions.Like(p.MarkdownContent, likeSearchTerm))
                                          .Where(p => p.PostedOn != null)
                                          .CountAsync(cancellationToken);

            await Task.WhenAll(resultsTask, postsCountTask).ConfigureAwait(false);

            return new ResultSetDto<PostDto>
            {
                Results = resultsTask.Result,
                TotalResults = postsCountTask.Result
            };
        }
    }
}
