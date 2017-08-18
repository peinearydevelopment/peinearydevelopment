using System.Threading.Tasks;

namespace DataAccess.Contracts.Blog
{
    public interface IPostsDal
    {
        Task<ResultSetDto<PostDto>> Search(int pageIndex, int pageSize);
        Task<ResultSetDto<PostDto>> Search(int pageIndex, int pageSize, string searchTerm);
    }
}
