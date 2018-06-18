using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageClient.Core.Extensions
{
    public static class PathExtensions
    {
        /// <summary>
        ///     Combine path with list of directories
        /// </summary>
        /// <param name="basePath">Base path</param>
        /// <param name="directories">List of directories</param>
        /// <param name="separator">Separator</param>
        /// <returns>Combined path</returns>
        public static string Combine(string basePath, IList<string> directories, string separator)
        {
            var builder = basePath.EndsWith(separator)
                ? new StringBuilder(basePath)
                : new StringBuilder(string.Concat(basePath, separator));

            foreach (var directory in directories) builder.Append(directory).Append(separator);

            return builder.ToString().TrimEnd(separator.ToCharArray());
        }

        /// <summary>
        ///     Get path seperated into parts
        /// </summary>
        /// <param name="path">Base path to seperate</param>
        /// <param name="separator">Separator</param>
        /// <param name="exclude">Path to remove from base path</param>
        /// <returns>Seperated path into directories and file</returns>
        public static IList<string> GetPathInParts(string path, char separator, string exclude = null)
        {
            if (!string.IsNullOrEmpty(exclude)) path = path.Replace(exclude, string.Empty);

            return path.Split(separator).Where(part => !string.IsNullOrEmpty(part)).ToList();
        }
    }
}