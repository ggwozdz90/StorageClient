using System;
using System.Collections.Generic;
using System.IO;

namespace StorageClient.Azure.Test.Helpers
{
    public static class DirectoryHelpers
    {
        public static IList<string> GetNewTestFolder(int numberOfFolders = 1)
        {
            IList<string> result = new List<string>();
            var mainPath = @"F:\Test";
            var runTime = DateTime.Now.ToString("MMddyyyyHHmmssfff");

            for (var i = 0; i < numberOfFolders; i++)
            {
                var path = Path.Combine(mainPath, runTime, i.ToString());
                Directory.CreateDirectory(path);
                result.Add(path);
            }

            return result;
        }
    }
}