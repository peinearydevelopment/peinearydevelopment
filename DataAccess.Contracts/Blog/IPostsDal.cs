using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccess.Contracts.Blog
{
    public interface IPostsDal
    {
        Task<PostDto> Read(Expression<Func<PostDto, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
        Task<PostDto[]> ReadMany(Expression<Func<PostDto, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
        Task<ResultSetDto<PostDto>> Search(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken));
        Task<ResultSetDto<PostDto>> Search(int pageIndex, int pageSize, string searchTerm, CancellationToken cancellationToken = default(CancellationToken));
        Task<PostDto> ReadPrevious(DateTimeOffset postedOn, CancellationToken cancellationToken = default(CancellationToken));
        Task<PostDto> ReadNext(DateTimeOffset postedOn, CancellationToken cancellationToken = default(CancellationToken));
        Task AddComment(int postId, CommentDto comment, CancellationToken cancellationToken = default(CancellationToken));
    }
}
