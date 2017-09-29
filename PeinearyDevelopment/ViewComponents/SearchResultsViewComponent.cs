using Contracts;
using Contracts.Blog;
using Microsoft.AspNetCore.Mvc;

namespace PeinearyDevelopment.ViewComponents
{
    public class SearchResultsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ResultSet<PostSummary> searchResults) => View("../SearchResults", searchResults);
    }
}
