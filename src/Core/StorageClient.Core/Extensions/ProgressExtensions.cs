using System;
using StorageClient.Core.Progress;

namespace StorageClient.Core.Extensions
{
    public static class ProgressExtensions
    {
        /// <summary>
        ///     Report directory creation
        /// </summary>
        /// <param name="progress">Progress notification</param>
        /// <param name="localPath">Local path</param>
        public static void ReportCreateDirectory(this IProgress<StorageProgressDto> progress, string localPath)
        {
            progress.Report(new StorageProgressDto(StorageProgressJobType.CreateDirectory, localPath));
        }

        /// <summary>
        ///     Report file saving
        /// </summary>
        /// <param name="progress">Progress notification</param>
        /// <param name="localPath">Local path</param>
        /// <param name="size">Size of file</param>
        public static void ReportSaveFile(this IProgress<StorageProgressDto> progress, string localPath, long size)
        {
            progress.Report(new StorageProgressDto(StorageProgressJobType.SaveFile, localPath, size));
        }

        /// <summary>
        ///     Report directory searching
        /// </summary>
        /// <param name="progress">Progress notification</param>
        /// <param name="storagePath">Storage path</param>
        public static void ReportSearchDirectory(this IProgress<StorageProgressDto> progress, string storagePath)
        {
            progress.Report(new StorageProgressDto(StorageProgressJobType.SearchDirectory, storagePath));
        }

        /// <summary>
        ///     Report listing of file entries in storage path
        /// </summary>
        /// <param name="progress">Progress notification</param>
        /// <param name="storagePath">Storage path</param>
        public static void ReportListFileEntries(this IProgress<StorageProgressDto> progress, string storagePath)
        {
            progress.Report(new StorageProgressDto(StorageProgressJobType.ListFileEntrt, storagePath));
        }

        /// <summary>
        ///     Report file downloading
        /// </summary>
        /// <param name="progress">Progress notification</param>
        /// <param name="storagePath">Storage path</param>
        /// <param name="fileName">File name</param>
        /// <param name="dataLength">Size of file</param>
        public static void ReportDownloadFile(this IProgress<StorageProgressDto> progress, string storagePath,
            string fileName, int dataLength)
        {
            progress.Report(new StorageProgressDto(StorageProgressJobType.DownloadFile, $"{storagePath}/{fileName}",
                dataLength));
        }

        /// <summary>
        ///     Reaport directory cleaning
        /// </summary>
        /// <param name="progress">Progress notification</param>
        /// <param name="localPath">Local path</param>
        public static void ReportCleanDirectory(this IProgress<StorageProgressDto> progress, string localPath)
        {
            progress.Report(new StorageProgressDto(StorageProgressJobType.CleanDirectory, localPath));
        }

        /// <summary>
        ///     Report file uploading
        /// </summary>
        /// <param name="progress">Progress notification</param>
        /// <param name="storagePath">Storage path</param>
        /// <param name="dataLength">Size of file</param>
        public static void ReportUploadFile(this IProgress<StorageProgressDto> progress, string storagePath,
            int dataLength)
        {
            progress.Report(new StorageProgressDto(StorageProgressJobType.UploadFile, storagePath, dataLength));
        }
    }
}