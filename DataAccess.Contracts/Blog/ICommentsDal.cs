using System.Threading;
using System.Threading.Tasks;

namespace DataAccess.Contracts.Blog
{
    public interface ICommentsDal
    {
        Task<CommentDto> Create(CommentDto comment, CancellationToken cancellationToken = default(CancellationToken));
    }
}
