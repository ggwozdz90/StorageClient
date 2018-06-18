using System.Collections.Generic;

namespace StorageClient.Core.Files
{
    public class FileEntryDto
    {
        public FileEntryDto(IList<string> path, long size = default)
        {
            Path = path;
            Size = size;
        }

        /// <summary>
        ///     Path in parts
        /// </summary>
        public IList<string> Path { get; }

        /// <summary>
        ///     Size of file
        /// </summary>
        public long Size { get; }
    }
}