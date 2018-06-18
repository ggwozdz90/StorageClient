using StorageClient.Core;
using StorageClient.Core.Directories;
using StorageClient.Core.Files;

namespace StorageClient.Azure
{
    public class AzureStorageClient : StorageClientBase
    {
        internal AzureStorageClient(IStorageClientProvider storageProvider, IDirectoryService directoryService,
            IFileService fileService) : base(storageProvider, directoryService, fileService)
        {
        }
    }
}