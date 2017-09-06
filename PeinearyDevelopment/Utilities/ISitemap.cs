using System.Threading.Tasks;

namespace PeinearyDevelopment.Utilities
{
    public interface ISitemap
    {
        Task<string> Generate();
    }
}
