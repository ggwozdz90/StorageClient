namespace StorageClient.Core.Progress
{
    public class StorageProgressDto
    {
        public StorageProgressDto(StorageProgressJobType storageProgressJobType, string path = default,
            long size = default)
        {
            StorageProgressJobType = storageProgressJobType;
            Path = path;
            Size = size;
        }

        /// <summary>
        ///     Storage job type
        /// </summary>
        public StorageProgressJobType StorageProgressJobType { get; }

        /// <summary>
        ///     Path
        /// </summary>
        public string Path { get; }

        /// <summary>
        ///     Size of file
        /// </summary>
        public long Size { get; }
    }
}