using AutoMapper;
using Contracts;
using Contracts.Blog;
using DataAccess.Contracts.Blog;
using Microsoft.AspNetCore.Mvc;
using PeinearyDevelopment.Utilities;
using System.Threading;
using System.Threading.Tasks;

namespace PeinearyDevelopment.Controllers
{
    public class BlogController : Controller
    {
        private IPostsDal PostsDal { get; }
        private IMapper Mapper { get; }
        private IRssFeed RssFeed { get; }
        private ISitemap SitemapGenerator { get; }

        public BlogController(IPostsDal postsDal, IMapper mapper, IRssFeed rssFeed, ISitemap sitemapGenerator)
        {
            PostsDal = postsDal;
            Mapper = mapper;
            RssFeed = rssFeed;
            SitemapGenerator = sitemapGenerator;
        }

        public async Task<IActionResult> Index(int id = 0, [FromQuery] int pageSize = 5, CancellationToken cancellationToken = default(CancellationToken))
        {
            var postSummaries = Mapper.Map<ResultSet<PostSummary>>(await PostsDal.Search(id, pageSize, cancellationToken).ConfigureAwait(false));
            postSummaries.PageSize = pageSize;
            postSummaries.PageIndex = id;

            return View(postSummaries);
        }

        public async Task<IActionResult> Post(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var post = Mapper.Map<Post>(await PostsDal.Read(p => p.Slug == id, cancellationToken).ConfigureAwait(false));
            post.PreviousPost = Mapper.Map<NavigationPost>(await PostsDal.ReadPrevious(post.PostedOn, cancellationToken).ConfigureAwait(false));
            post.NextPost = Mapper.Map<NavigationPost>(await PostsDal.ReadNext(post.PostedOn, cancellationToken).ConfigureAwait(false));
            ViewBag.Title = post.Title;
            return View(post);
        }

        public async Task<ContentResult> Rss(CancellationToken cancellationToken = default(CancellationToken)) => Content(await RssFeed.Generate(cancellationToken).ConfigureAwait(false), "application/xml");
        public async Task<ContentResult> Sitemap(CancellationToken cancellationToken = default(CancellationToken)) => Content(await SitemapGenerator.Generate(cancellationToken).ConfigureAwait(false), "application/xml");
    }
}
