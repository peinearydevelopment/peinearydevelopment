using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PeinearyDevelopment.Services
{
    public class FilesService : IFilesService
    {
        private CloudBlobContainer CloudBlobContainer { get; }
        private IMemoryCache MemoryCache { get; }

        public FilesService(IConfigurationRoot configurationRoot, IMemoryCache memoryCache)
        {
            MemoryCache = memoryCache;
            var cloudStorageAccount = CloudStorageAccount.Parse(configurationRoot["ConnectionStrings:CloudStorageAccountConnectionString"]);
            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer = cloudBlobClient.GetContainerReference(configurationRoot["CloudStorage:BlogFilesContainerName"]);
        }

        public async Task<byte[]> GetFileContents(string fileId)
        {
            var cachedFile = MemoryCache.Get<byte[]>(fileId);
            if(cachedFile != null)
            {
                return cachedFile;
            }

            var cloudBlob = await CloudBlobContainer.GetBlobReferenceFromServerAsync(fileId).ConfigureAwait(false);
            var memoryStream = new MemoryStream();
            await cloudBlob.DownloadToStreamAsync(memoryStream).ConfigureAwait(false);
            var fileContents = memoryStream.ToArray();
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(365));
            MemoryCache.Set(fileId, fileContents, cacheEntryOptions);

            return fileContents;
        }
    }
}
