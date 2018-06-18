namespace StorageClient.Core.Settings
{
    public interface IStorageSettings
    {
        /// <summary>
        ///     Number of concurrent downloading
        /// </summary>
        int ConcurrentDownload { get; set; }

        /// <summary>
        ///     Number of concurrent uploading
        /// </summary>
        int ConcurrentUpload { get; set; }

        /// <summary>
        ///     Connection string to storage provider
        /// </summary>
        string ConnectionString { get; set; }
    }
}