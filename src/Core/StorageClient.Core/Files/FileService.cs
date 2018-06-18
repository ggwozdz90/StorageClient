using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StorageClient.Core.Extensions;
using StorageClient.Core.Progress;

namespace StorageClient.Core.Files
{
    public class FileService : IFileService
    {
        private readonly IStorageClientProvider _storageProvider;

        public FileService(IStorageClientProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }

        public async Task CreateFilesAsync(
            string localPath,
            string storagePath,
            IList<FileEntryDto> files,
            IProgress<StorageProgressDto> progress,
            CancellationToken cancellationToken)
        {
            using (var semaphore = new SemaphoreSlim(
                _storageProvider.StorageSettings.ConcurrentDownload,
                _storageProvider.StorageSettings.ConcurrentDownload))
            {
                var tasks = files.Select(
                        CreateConcurrentFilesAsync(localPath, storagePath, semaphore, progress, cancellationToken))
                    .ToArray();

                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
        }

        public async Task UploadFilesAsync(
            string localPath,
            string storagePath,
            IEnumerable<FileEntryDto> files,
            IProgress<StorageProgressDto> progress,
            CancellationToken cancellationToken)
        {
            using (var semaphore = new SemaphoreSlim(
                _storageProvider.StorageSettings.ConcurrentUpload,
                _storageProvider.StorageSettings.ConcurrentUpload))
            {
                var tasks = files.Select(
                        UploadConcurrentFilesAsync(localPath, storagePath, semaphore, progress, cancellationToken))
                    .ToArray();

                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
        }

        public async Task CreateFileAsync(
            string localPath,
            string storagePath,
            IProgress<StorageProgressDto> progress,
            CancellationToken cancellationToken)
        {
            var pathInParts = PathExtensions.GetPathInParts(storagePath, '/');
            var fileName = pathInParts.PopLast();
            var data = await _storageProvider.DownloadFileAsync(PathExtensions.Combine(string.Empty, pathInParts, "/"),
                    fileName, progress, cancellationToken)
                .ConfigureAwait(false);

            using (var fileStream = File.Open($"{localPath.TrimEnd('\\')}\\{fileName}", FileMode.Create))
            {
                cancellationToken.ThrowIfCancellationRequested();
                progress.ReportSaveFile(localPath, data.Length);

                await fileStream.WriteAsync(data, 0, data.Length, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task UploadFileAsync(
            string localPath,
            string storagePath,
            IProgress<StorageProgressDto> progress,
            CancellationToken cancellationToken)
        {
            var data = File.ReadAllBytes(localPath);
            var pathInParts = PathExtensions.GetPathInParts(storagePath, '/');
            var fileName = pathInParts.PopLast();
            var fullStoragePath = PathExtensions.Combine(string.Empty, pathInParts, "/");

            await _storageProvider.UploadDirectoriesAsync(pathInParts.PopFirst(), new FileEntryDto(pathInParts),
                progress,
                cancellationToken).ConfigureAwait(false);

            await _storageProvider
                .UploadFileAsync(data, fullStoragePath, fileName, progress, cancellationToken)
                .ConfigureAwait(false);
        }

        private Func<FileEntryDto, Task> UploadConcurrentFilesAsync(
            string localPath,
            string storagePath,
            SemaphoreSlim semaphore,
            IProgress<StorageProgressDto> progress,
            CancellationToken cancellationToken)
        {
            return async directory =>
            {
                await semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
                try
                {
                    var data = File.ReadAllBytes(PathExtensions.Combine(localPath, directory.Path, "\\"));
                    var fileName = directory.Path.PopLast();
                    var fullStoragePath = PathExtensions.Combine(storagePath, directory.Path, "/");

                    await _storageProvider
                        .UploadFileAsync(data, fullStoragePath, fileName, progress, cancellationToken)
                        .ConfigureAwait(false);
                }
                finally
                {
                    semaphore.Release();
                }
            };
        }

        private Func<FileEntryDto, Task> CreateConcurrentFilesAsync(
            string localPath,
            string storagePath,
            SemaphoreSlim semaphore,
            IProgress<StorageProgressDto> progress,
            CancellationToken cancellationToken)
        {
            return async file =>
            {
                await semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
                try
                {
                    await CreateFileAsync(localPath, storagePath, file, progress, cancellationToken)
                        .ConfigureAwait(false);
                }
                finally
                {
                    semaphore.Release();
                }
            };
        }

        private async Task CreateFileAsync(
            string localPath,
            string storagePath,
            FileEntryDto file,
            IProgress<StorageProgressDto> progress,
            CancellationToken cancellationToken)
        {
            var fullLocalPath = PathExtensions.Combine(localPath, file.Path, "\\");
            var fileName = file.Path.PopLast();
            var fullStoragePath = PathExtensions.Combine(storagePath, file.Path, "/");
            var data = await _storageProvider.DownloadFileAsync(fullStoragePath, fileName, progress, cancellationToken)
                .ConfigureAwait(false);

            using (var fileStream = File.Open(fullLocalPath, FileMode.Create))
            {
                cancellationToken.ThrowIfCancellationRequested();
                progress.ReportSaveFile(fullLocalPath, data.Length);

                await fileStream.WriteAsync(data, 0, data.Length, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}