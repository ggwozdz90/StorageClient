using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StorageClient.Core.Extensions;
using StorageClient.Core.Files;
using StorageClient.Core.Progress;

namespace StorageClient.Core.Directories
{
    public class DirectoryService : IDirectoryService
    {
        private readonly IStorageClientProvider _storageProvider;

        public DirectoryService(IStorageClientProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }

        public async Task CreateDirectoriesAsync(
            string path,
            IList<FileEntryDto> directories,
            IProgress<StorageProgressDto> progress,
            CancellationToken cancellationToken)
        {
            await Task.Run(() =>
                        Parallel.ForEach(directories,
                            directory =>
                            {
                                var fullLocalPath = PathExtensions.Combine(path, directory.Path, "\\");
                                progress.ReportCreateDirectory(fullLocalPath);
                                Directory.CreateDirectory(fullLocalPath);
                            }),
                    cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task DeleteDirectoryAsync(
            string path,
            IProgress<StorageProgressDto> progress)
        {
            progress.ReportCleanDirectory(path);

            await Task.Run(() =>
            {
                if (Directory.Exists(path))
                {
                    var directoryInfo = new DirectoryInfo(path);

                    foreach (var dir in directoryInfo.EnumerateDirectories()) dir.Delete(true);
                    foreach (var file in directoryInfo.EnumerateFiles()) file.Delete();
                }
            }).ConfigureAwait(false);
        }

        public (IList<FileEntryDto> Directories, IList<FileEntryDto> Files) ListFileEntries(
            string path,
            IProgress<StorageProgressDto> progress,
            CancellationToken cancellationToken)
        {
            var directories = new ConcurrentBag<FileEntryDto>();
            var files = new ConcurrentBag<FileEntryDto>();

            cancellationToken.ThrowIfCancellationRequested();
            progress.ReportSearchDirectory(path);

            var fileEntires = Directory.GetFileSystemEntries(path, "*.*", SearchOption.AllDirectories);

            Parallel.ForEach(fileEntires,
                fileEntry =>
                {
                    var pathInParts = PathExtensions.GetPathInParts(fileEntry, '\\', path);

                    if (FileExtensions.HasDirectoryFlag(fileEntry))
                        directories.Add(new FileEntryDto(pathInParts));
                    else
                        files.Add(new FileEntryDto(pathInParts, new FileInfo(fileEntry).Length));
                });

            return (directories.ToList(), files.ToList());
        }

        public async Task UploadDirectoriesAsync(
            string path,
            IEnumerable<FileEntryDto> directories,
            IProgress<StorageProgressDto> progress,
            CancellationToken cancellationToken)
        {
            using (var semaphore = new SemaphoreSlim(
                _storageProvider.StorageSettings.ConcurrentUpload,
                _storageProvider.StorageSettings.ConcurrentUpload))
            {
                var tasks = directories.Select(
                        UploadConcurrentDirectoriesAsync(path, semaphore, progress, cancellationToken))
                    .ToArray();

                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
        }

        private Func<FileEntryDto, Task> UploadConcurrentDirectoriesAsync(
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
                    await _storageProvider.UploadDirectoriesAsync(storagePath, directory, progress, cancellationToken)
                        .ConfigureAwait(false);
                }
                finally
                {
                    semaphore.Release();
                }
            };
        }
    }
}