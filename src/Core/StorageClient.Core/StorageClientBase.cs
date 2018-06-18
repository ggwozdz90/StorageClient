using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StorageClient.Core.Directories;
using StorageClient.Core.Extensions;
using StorageClient.Core.Files;
using StorageClient.Core.Progress;

namespace StorageClient.Core
{
    public abstract class StorageClientBase : IStorageClient
    {
        private readonly IDirectoryService _directoryService;
        private readonly IFileService _fileService;
        private readonly IStorageClientProvider _storageProvider;

        protected StorageClientBase(
            IStorageClientProvider storageProvider,
            IDirectoryService directoryService,
            IFileService fileService)
        {
            _storageProvider = storageProvider;
            _directoryService = directoryService;
            _fileService = fileService;
        }

        public virtual async Task DownloadDirectoryAsync(
            string localPath,
            string storagePath,
            IProgress<StorageProgressDto> progress = default,
            CancellationToken cancellationToken = default)
        {
            CheckPaths(localPath, storagePath);

            if (!Directory.Exists(localPath))
                Directory.CreateDirectory(localPath);

            if (!DirectoryExtensions.IsDirectoryEmpty(localPath))
                throw new IOException($"The directory '{localPath}' is not empty.");

            try
            {
                var (directories, files) =
                    await _storageProvider.ListFileEntriesAsync(storagePath, progress, cancellationToken)
                        .ConfigureAwait(false);

                if (directories.Any())
                    await _directoryService.CreateDirectoriesAsync(localPath, directories, progress, cancellationToken)
                        .ConfigureAwait(false);

                if (files.Any())
                    await _fileService.CreateFilesAsync(localPath, storagePath, files, progress, cancellationToken)
                        .ConfigureAwait(false);
            }
            catch (Exception)
            {
                await _directoryService.DeleteDirectoryAsync(localPath, progress).ConfigureAwait(false);
                throw;
            }
        }

        public async Task UploadDirectoryAsync(
            string localPath,
            string storagePath,
            IProgress<StorageProgressDto> progress = default,
            CancellationToken cancellationToken = default)
        {
            CheckPaths(localPath, storagePath);

            var (directories, files) =
                _directoryService.ListFileEntries(localPath, progress, cancellationToken);

            if (directories.Any())
                await _directoryService.UploadDirectoriesAsync(storagePath, directories, progress, cancellationToken)
                    .ConfigureAwait(false);

            if (files.Any())
                await _fileService.UploadFilesAsync(localPath, storagePath, files, progress, cancellationToken)
                    .ConfigureAwait(false);
        }

        public async Task DownloadFileAsync(
            string localPath,
            string storagePath,
            IProgress<StorageProgressDto> progress = default,
            CancellationToken cancellationToken = default)
        {
            CheckPaths(localPath, storagePath);

            if (!Directory.Exists(localPath))
                Directory.CreateDirectory(localPath);

            await _fileService.CreateFileAsync(localPath, storagePath, progress, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task UploadFileAsync(
            string localPath,
            string storagePath,
            IProgress<StorageProgressDto> progress = default,
            CancellationToken cancellationToken = default)
        {
            CheckPaths(localPath, storagePath);

            if (!File.Exists(localPath))
                throw new FileNotFoundException($"File {localPath} not exists");

            await _fileService.UploadFileAsync(localPath, storagePath, progress, cancellationToken)
                .ConfigureAwait(false);
        }

        private static void CheckPaths(string localPath, string storagePath)
        {
            if (string.IsNullOrEmpty(localPath))
                throw new ArgumentNullException(localPath);

            if (string.IsNullOrEmpty(storagePath))
                throw new ArgumentNullException(storagePath);
        }
    }
}