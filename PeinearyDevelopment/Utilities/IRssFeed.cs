using System.Threading.Tasks;

namespace PeinearyDevelopment.Utilities
{
    public interface IRssFeed
    {
        Task<string> Generate();
    }
}
