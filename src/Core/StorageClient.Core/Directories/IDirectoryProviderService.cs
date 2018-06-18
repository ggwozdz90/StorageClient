using System;
using System.Threading;
using System.Threading.Tasks;
using StorageClient.Core.Files;
using StorageClient.Core.Progress;

namespace StorageClient.Core.Directories
{
    public interface IProviderDirectoryService
    {
        /// <summary>
        ///     Find directory on storage provider and return reference.
        /// </summary>
        /// <param name="path">Path to directory</param>
        /// <param name="progress">Progress notification</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Reference to storage file directory</returns>
        object FindFileDirectory(string path,
            IProgress<StorageProgressDto> progress, CancellationToken cancellationToken);

        /// <summary>
        ///     Create directory on storage provider
        /// </summary>
        /// <param name="path">Base path</param>
        /// <param name="directory">Directories to create if not exists</param>
        /// <param name="storageFileDirectory">Reference to storage file directory</param>
        /// <param name="progress">Progress notification</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task</returns>
        Task UploadFileDirectory(string path, FileEntryDto directory, object storageFileDirectory,
            IProgress<StorageProgressDto> progress, CancellationToken cancellationToken);
    }
}