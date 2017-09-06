using AutoMapper;
using Contracts;
using Contracts.Blog;
using DataAccess.Contracts;
using DataAccess.Contracts.Blog;
using Microsoft.AspNetCore.Mvc;
using PeinearyDevelopment.Utilities;
using System.Threading.Tasks;

namespace PeinearyDevelopment.Controllers
{
    public class BlogController : Controller
    {
        private IPostsDal PostsDal { get; }
        private IMapper Mapper { get; }
        private IRssFeed RssFeed { get; }

        public BlogController(IPostsDal postsDal, IMapper mapper, IRssFeed rssFeed)
        {
            PostsDal = postsDal;
            Mapper = mapper;
            RssFeed = rssFeed;
        }

        public async Task<IActionResult> Index([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 5)
        {
            var postSummaries = Mapper.Map<ResultSet<PostSummary>>(await PostsDal.Search(pageIndex, pageSize).ConfigureAwait(false));
            postSummaries.PageSize = pageSize;
            postSummaries.PageIndex = pageIndex;

            return View(postSummaries);
        }

        public async Task<IActionResult> Post(string id)
        {
            var post = Mapper.Map<Post>(await PostsDal.Read(p => p.Slug == id).ConfigureAwait(false));
            post.PreviousPost = Mapper.Map<NavigationPost>(await PostsDal.ReadPrevious(post.PostedOn).ConfigureAwait(false));
            post.NextPost = Mapper.Map<NavigationPost>(await PostsDal.ReadNext(post.PostedOn).ConfigureAwait(false));

            return View(post);
        }

        public async Task<ContentResult> Rss() => Content(await RssFeed.Generate().ConfigureAwait(false), "application/xml");
    }
}
