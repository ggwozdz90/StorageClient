using System;
using System.Threading;
using System.Threading.Tasks;
using StorageClient.Core.Progress;

namespace StorageClient.Core
{
    public interface IStorageClient
    {
        /// <summary>
        ///     Download directory with whole content from storage provider
        /// </summary>
        /// <param name="localPath">Local path</param>
        /// <param name="storagePath">Storage path</param>
        /// <param name="progress">Progress notification</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task</returns>
        Task DownloadDirectoryAsync(string localPath, string storagePath,
            IProgress<StorageProgressDto> progress = default, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Upload directory with whole content to storage provider
        /// </summary>
        /// <param name="localPath">Local path</param>
        /// <param name="storagePath">Storage path</param>
        /// <param name="progress">Progress notification</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task</returns>
        Task UploadDirectoryAsync(string localPath, string storagePath,
            IProgress<StorageProgressDto> progress = default, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Download file from storage provider
        /// </summary>
        /// <param name="localPath">Local path</param>
        /// <param name="storagePath">Storage path</param>
        /// <param name="progress">Progress notification</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task</returns>
        Task DownloadFileAsync(string localPath, string storagePath,
            IProgress<StorageProgressDto> progress = default, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Upload file to storage provider
        /// </summary>
        /// <param name="localPath">Local path</param>
        /// <param name="storagePath">Storage path</param>
        /// <param name="progress">Progress notification</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task</returns>
        Task UploadFileAsync(string localPath, string storagePath,
            IProgress<StorageProgressDto> progress = default, CancellationToken cancellationToken = default);
    }
}