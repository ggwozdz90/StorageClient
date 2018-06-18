namespace StorageClient.Core.Settings
{
    public class StorageSettings : IStorageSettings
    {
        public string ConnectionString { get; set; }
        public int ConcurrentUpload { get; set; } = 3;
        public int ConcurrentDownload { get; set; } = 3;
    }
}