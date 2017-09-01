using System.Threading.Tasks;

namespace DataAccess.Contracts
{
    public interface IPageViewsDal
    {
        Task Create(PageViewDto pageView);
    }
}
