using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StorageClient.Core.Files;
using StorageClient.Core.Progress;
using StorageClient.Core.Settings;

namespace StorageClient.Core
{
    public interface IStorageClientProvider
    {
        /// <summary>
        ///     Storage settings
        /// </summary>
        IStorageSettings StorageSettings { get; }

        /// <summary>
        ///     List file entries
        /// </summary>
        /// <param name="path">Storage path</param>
        /// <param name="progress">Progress notification</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Tuple ith list of directories and files</returns>
        Task<(IList<FileEntryDto> Directories, IList<FileEntryDto> Files)> ListFileEntriesAsync(string path,
            IProgress<StorageProgressDto> progress, CancellationToken cancellationToken);

        /// <summary>
        ///     Download file from storage provider
        /// </summary>
        /// <param name="path">Storage path</param>
        /// <param name="fileName">File name</param>
        /// <param name="progress">Progress notification</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Byte array with file content</returns>
        Task<byte[]> DownloadFileAsync(string path, string fileName,
            IProgress<StorageProgressDto> progress, CancellationToken cancellationToken);

        /// <summary>
        ///     Upload directories to storage provider
        /// </summary>
        /// <param name="path">Storage path</param>
        /// <param name="directory">Directory to create</param>
        /// <param name="progress">Progress notification</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task</returns>
        Task UploadDirectoriesAsync(string path, FileEntryDto directory,
            IProgress<StorageProgressDto> progress, CancellationToken cancellationToken);

        /// <summary>
        ///     Upload file to storage provider
        /// </summary>
        /// <param name="data">Byte array with file content</param>
        /// <param name="path">Storage path</param>
        /// <param name="fileName">Directory to create</param>
        /// <param name="progress">Progress notification</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task</returns>
        Task UploadFileAsync(byte[] data, string path, string fileName,
            IProgress<StorageProgressDto> progress, CancellationToken cancellationToken);
    }
}