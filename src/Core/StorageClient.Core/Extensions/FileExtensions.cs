using System.IO;

namespace StorageClient.Core.Extensions
{
    public static class FileExtensions
    {
        /// <summary>
        ///     Check if file entry is directory
        /// </summary>
        /// <param name="fileEntry">Path to file or directory</param>
        /// <returns>True if file entry is directory, otherwise false</returns>
        public static bool HasDirectoryFlag(string fileEntry)
        {
            return File.GetAttributes(fileEntry).HasFlag(FileAttributes.Directory);
        }
    }
}