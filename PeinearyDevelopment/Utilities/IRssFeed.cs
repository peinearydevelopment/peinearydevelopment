using System.Threading;
using System.Threading.Tasks;

namespace PeinearyDevelopment.Utilities
{
    public interface IRssFeed
    {
        Task<string> Generate(CancellationToken cancellationToken = default(CancellationToken));
    }
}
