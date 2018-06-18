using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StorageClient.Core.Progress;

namespace StorageClient.Core.Files
{
    public interface IProviderFileService
    {
        /// <summary>
        ///     List file entries in storage provider
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="path">Storage path</param>
        /// <param name="directory">Directory reference</param>
        /// <param name="progress">Progress notification</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Tuple with list of directories and files</returns>
        Task<(IList<FileEntryDto> Directories, IList<FileEntryDto> Files)> ListFileEntriesAsync<T>(
            string path, T directory,
            IProgress<StorageProgressDto> progress, CancellationToken cancellationToken);

        /// <summary>
        ///     Download file from storage provider
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="fileName">File name</param>
        /// <param name="directory">Directory reference</param>
        /// <param name="progress">Progress notification</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Byte array with file content</returns>
        Task<byte[]> DownloadFileAsync<T>(string fileName, T directory,
            IProgress<StorageProgressDto> progress, CancellationToken cancellationToken);

        /// <summary>
        ///     Upload file to storage provider
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="data">Byte array with file content</param>
        /// <param name="fileName">File name</param>
        /// <param name="directory">Directory reference</param>
        /// <param name="progress">Progress notification</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        Task UploadFileAsync<T>(byte[] data, string fileName, T directory,
            IProgress<StorageProgressDto> progress, CancellationToken cancellationToken);
    }
}