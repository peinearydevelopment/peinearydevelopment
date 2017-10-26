using System.Threading.Tasks;

namespace PeinearyDevelopment.Services
{
    public interface IFilesService
    {
        Task<byte[]> GetFileContents(string fileId);
    }
}
