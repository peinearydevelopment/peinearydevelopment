using AutoMapper;
using Contracts;
using Contracts.Blog;
using DataAccess.Contracts.Blog;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PeinearyDevelopment.Controllers
{
    public class BlogController : Controller
    {
        private IPostsDal PostsDal { get; }
        private IMapper Mapper { get; }

        public BlogController(IPostsDal postsDal, IMapper mapper)
        {
            PostsDal = postsDal;
            Mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(Mapper.Map<ResultSet<PostSummary>>(await PostsDal.Search(0, 5).ConfigureAwait(false)));
        }
    }
}
