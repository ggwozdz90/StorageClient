using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.File;
using StorageClient.Core.Directories;
using StorageClient.Core.Extensions;
using StorageClient.Core.Files;
using StorageClient.Core.Progress;

namespace StorageClient.Provider.Azure.Directories
{
    public class AzureDirectoryService : IProviderDirectoryService
    {
        private readonly CloudFileClient _azureFileClient;

        public AzureDirectoryService(CloudFileClient azureFileClient)
        {
            _azureFileClient = azureFileClient;
        }

        public object FindFileDirectory(
            string path,
            IProgress<StorageProgressDto> progress,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            progress.ReportSearchDirectory(path);

            var directories = PathExtensions.GetPathInParts(path, '/');
            var share = _azureFileClient.GetShareReference(directories.PopFirst());
            var root = share.GetRootDirectoryReference();
            return FindStorageFileDirectoryRecursive(root, directories);
        }

        public async Task UploadFileDirectory(
            string path,
            FileEntryDto directory,
            object storageFileDirectory,
            IProgress<StorageProgressDto> progress,
            CancellationToken cancellationToken)
        {
            if (directory.Path.Any())
            {
                var firstdirectory = directory.Path.PopFirst();
                var newPath = $"{path}/{firstdirectory}";

                cancellationToken.ThrowIfCancellationRequested();
                progress.ReportCreateDirectory(newPath);

                CloudFileDirectory directoryReference = null;

                if (storageFileDirectory is CloudFileDirectory storageDirectory)
                {
                    directoryReference = storageDirectory.GetDirectoryReference(firstdirectory);
                    await directoryReference.CreateIfNotExistsAsync().ConfigureAwait(false);
                }

                await UploadFileDirectory(newPath, directory, directoryReference, progress, cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        private CloudFileDirectory FindStorageFileDirectoryRecursive(
            CloudFileDirectory storageFileDirectory,
            IList<string> directories)
        {
            if (directories.Any())
            {
                storageFileDirectory = storageFileDirectory.GetDirectoryReference(directories.PopFirst());
                storageFileDirectory = FindStorageFileDirectoryRecursive(storageFileDirectory, directories);
            }

            return storageFileDirectory;
        }
    }
}