using System.IO;
using System.Linq;

namespace StorageClient.Core.Extensions
{
    public static class DirectoryExtensions
    {
        /// <summary>
        ///     Check if directory has content
        /// </summary>
        /// <param name="path">Path to check</param>
        /// <returns>True if has content, otherwise false</returns>
        public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }
    }
}