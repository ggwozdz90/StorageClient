using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StorageClient.Core.Progress;

namespace StorageClient.Core.Files
{
    public interface IFileService
    {
        /// <summary>
        ///     Create files in local path
        /// </summary>
        /// <param name="localPath">Local path</param>
        /// <param name="storagePath">Storage path</param>
        /// <param name="files">List of files</param>
        /// <param name="progress">Progress notificaion</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task</returns>
        Task CreateFilesAsync(string localPath, string storagePath, IList<FileEntryDto> files,
            IProgress<StorageProgressDto> progress, CancellationToken cancellationToken);

        /// <summary>
        ///     Upload files from local path
        /// </summary>
        /// <param name="localPath">Local path</param>
        /// <param name="storagePath">Storage path</param>
        /// <param name="files">List of files</param>
        /// <param name="progress">Progress notificaion</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task</returns>
        Task UploadFilesAsync(string localPath, string storagePath, IEnumerable<FileEntryDto> files,
            IProgress<StorageProgressDto> progress, CancellationToken cancellationToken);

        /// <summary>
        ///     Create file in local path
        /// </summary>
        /// <param name="localPath">Local path</param>
        /// <param name="storagePath">Storage path</param>
        /// <param name="progress">Progress notificaion</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task</returns>
        Task CreateFileAsync(string localPath, string storagePath,
            IProgress<StorageProgressDto> progress, CancellationToken cancellationToken);

        /// <summary>
        ///     Upload file from local path
        /// </summary>
        /// <param name="localPath">Local path</param>
        /// <param name="storagePath">Storage path</param>
        /// <param name="progress">Progress notificaion</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task</returns>
        Task UploadFileAsync(string localPath, string storagePath,
            IProgress<StorageProgressDto> progress, CancellationToken cancellationToken);
    }
}