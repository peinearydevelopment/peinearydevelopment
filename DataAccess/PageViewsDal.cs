using DataAccess.Contracts;
using System.Threading.Tasks;

namespace DataAccess
{
    public class PageViewsDal : IPageViewsDal
    {
        private PdDbContext DbContext { get; }

        public PageViewsDal(PdDbContext dbContext) => DbContext = dbContext;

        public async Task Create(PageViewDto pageView)
        {
            await DbContext.PageViews.AddAsync(pageView).ConfigureAwait(false);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
