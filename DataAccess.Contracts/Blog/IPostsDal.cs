using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess.Contracts.Blog
{
    public interface IPostsDal
    {
        Task<PostDto> Read(Expression<Func<PostDto, bool>> predicate);
        Task<PostDto[]> ReadMany(Expression<Func<PostDto, bool>> predicate);
        Task<ResultSetDto<PostDto>> Search(int pageIndex, int pageSize);
        Task<ResultSetDto<PostDto>> Search(int pageIndex, int pageSize, string searchTerm);
        Task<PostDto> ReadPrevious(DateTimeOffset postedOn);
        Task<PostDto> ReadNext(DateTimeOffset postedOn);
    }
}
