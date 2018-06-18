using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StorageClient.Core;
using StorageClient.Core.Directories;
using StorageClient.Core.Files;
using StorageClient.Core.Progress;
using StorageClient.Core.Settings;

namespace StorageClient.Provider.Azure
{
    public class StorageClientProviderAzure : IStorageClientProvider
    {
        private readonly IProviderDirectoryService _azureDirectoryService;
        private readonly IProviderFileService _azureFileService;

        public StorageClientProviderAzure(IProviderFileService azureFileService,
            IProviderDirectoryService azureDirectoryService, IStorageSettings storageSettings)
        {
            _azureFileService = azureFileService;
            _azureDirectoryService = azureDirectoryService;
            StorageSettings = storageSettings;
        }

        public IStorageSettings StorageSettings { get; }

        public async Task<(IList<FileEntryDto> Directories, IList<FileEntryDto> Files)> ListFileEntriesAsync(
            string path,
            IProgress<StorageProgressDto> progress,
            CancellationToken cancellationToken)
        {
            var azureFileDirectory =
                _azureDirectoryService.FindFileDirectory(path, progress, cancellationToken);

            return await _azureFileService.ListFileEntriesAsync(path, azureFileDirectory, progress, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<byte[]> DownloadFileAsync(
            string path,
            string fileName,
            IProgress<StorageProgressDto> progress,
            CancellationToken cancellationToken)
        {
            var azureFileDirectory =
                _azureDirectoryService.FindFileDirectory(path, progress, cancellationToken);

            return await _azureFileService.DownloadFileAsync(fileName, azureFileDirectory, progress, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task UploadDirectoriesAsync(
            string path,
            FileEntryDto directory,
            IProgress<StorageProgressDto> progress,
            CancellationToken cancellationToken)
        {
            var azureFileDirectory =
                _azureDirectoryService.FindFileDirectory(path, progress, cancellationToken);

            await _azureDirectoryService
                .UploadFileDirectory(path, directory, azureFileDirectory, progress, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task UploadFileAsync(
            byte[] data,
            string path,
            string fileName,
            IProgress<StorageProgressDto> progress,
            CancellationToken cancellationToken)
        {
            var azureFileDirectory =
                _azureDirectoryService.FindFileDirectory(path, progress, cancellationToken);

            await _azureFileService.UploadFileAsync(data, fileName, azureFileDirectory, progress, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}