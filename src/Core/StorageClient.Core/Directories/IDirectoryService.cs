using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StorageClient.Core.Files;
using StorageClient.Core.Progress;

namespace StorageClient.Core.Directories
{
    public interface IDirectoryService
    {
        /// <summary>
        ///     Create directorie in local path
        /// </summary>
        /// <param name="path">Local path</param>
        /// <param name="directories">List of directories to create</param>
        /// <param name="progress">Progress notification</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task</returns>
        Task CreateDirectoriesAsync(string path, IList<FileEntryDto> directories,
            IProgress<StorageProgressDto> progress, CancellationToken cancellationToken);

        /// <summary>
        ///     Delete content of directory
        /// </summary>
        /// <param name="path">Directory path</param>
        /// <param name="progress">Progress notification</param>
        /// <returns>Task</returns>
        Task DeleteDirectoryAsync(string path, IProgress<StorageProgressDto> progress);

        /// <summary>
        ///     List directories and files from path
        /// </summary>
        /// <param name="path">Directory path</param>
        /// <param name="progress">Progress notification</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Tuple with list of directories and files</returns>
        (IList<FileEntryDto> Directories, IList<FileEntryDto> Files) ListFileEntries(string path,
            IProgress<StorageProgressDto> progress, CancellationToken cancellationToken);

        /// <summary>
        ///     Create directories on storage provider
        /// </summary>
        /// <param name="path">Storage path</param>
        /// <param name="directories">List of directoires to create on storage provider</param>
        /// <param name="progress">Progress notification</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task</returns>
        Task UploadDirectoriesAsync(string path, IEnumerable<FileEntryDto> directories,
            IProgress<StorageProgressDto> progress, CancellationToken cancellationToken);
    }
}