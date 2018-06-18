using Microsoft.WindowsAzure.Storage;
using StorageClient.Core.Directories;
using StorageClient.Core.Files;
using StorageClient.Core.Settings;
using StorageClient.Provider.Azure;
using StorageClient.Provider.Azure.Directories;
using StorageClient.Provider.Azure.Files;

namespace StorageClient.Azure
{
    public static class AzureStorageClientBuilder
    {
        /// <summary>
        ///     Azure storage client builder
        /// </summary>
        /// <param name="storageSettings">Storage settings</param>
        /// <returns>AzureStorageClient</returns>
        public static AzureStorageClient Build(IStorageSettings storageSettings)
        {
            var account = CloudStorageAccount.Parse(storageSettings.ConnectionString);
            var storageFileClient = account.CreateCloudFileClient();

            var azureFileService = new AzureFileService();
            var azureDirectoryService = new AzureDirectoryService(storageFileClient);
            var storageClientAzureProvider =
                new StorageClientProviderAzure(azureFileService, azureDirectoryService, storageSettings);

            var directoryService = new DirectoryService(storageClientAzureProvider);
            var fileService = new FileService(storageClientAzureProvider);

            return new AzureStorageClient(storageClientAzureProvider, directoryService, fileService);
        }
    }
}