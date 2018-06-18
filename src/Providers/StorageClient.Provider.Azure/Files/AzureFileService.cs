using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using StorageClient.Core.Extensions;
using StorageClient.Core.Files;
using StorageClient.Core.Progress;

namespace StorageClient.Provider.Azure.Files
{
    public class AzureFileService : IProviderFileService
    {
        public async Task<(IList<FileEntryDto> Directories, IList<FileEntryDto> Files)> ListFileEntriesAsync<T>(
            string path,
            T directory,
            IProgress<StorageProgressDto> progress,
            CancellationToken cancellationToken)
        {
            var directories = new List<FileEntryDto>();
            var files = new List<FileEntryDto>();
            var azureFileDirectory = directory as CloudFileDirectory;

            cancellationToken.ThrowIfCancellationRequested();

            if (azureFileDirectory != null)
            {
                progress.ReportListFileEntries(azureFileDirectory.Uri.LocalPath);

                var azureFileEntries = await azureFileDirectory.ListFilesAndDirectoriesSegmentedAsync(null,
                    new FileContinuationToken(), new FileRequestOptions(), new OperationContext(),
                    cancellationToken).ConfigureAwait(false);

                foreach (var azurefileEntry in azureFileEntries.Results)
                    switch (azurefileEntry)
                    {
                        case CloudFileDirectory storageFileDirectory:
                            var subDirectory =
                                await ListFileEntriesAsync(path, storageFileDirectory, progress, cancellationToken)
                                    .ConfigureAwait(false);

                            directories.Add(new FileEntryDto(
                                PathExtensions.GetPathInParts(storageFileDirectory.Uri.LocalPath, '/', path)));

                            files.AddRange(subDirectory.Files);
                            directories.AddRange(subDirectory.Directories);
                            break;
                        case CloudFile cloudFile:
                            await cloudFile.FetchAttributesAsync().ConfigureAwait(false);
                            files.Add(new FileEntryDto(
                                PathExtensions.GetPathInParts(azurefileEntry.Uri.LocalPath, '/', path),
                                cloudFile.Properties.Length));
                            break;
                    }
            }

            return (directories, files);
        }

        public async Task<byte[]> DownloadFileAsync<T>(
            string fileName,
            T directory,
            IProgress<StorageProgressDto> progress,
            CancellationToken cancellationToken)
        {
            var fileDirectory = directory as CloudFileDirectory;

            if (fileDirectory != null)
            {
                var fileReference = fileDirectory.GetFileReference(fileName);
                await fileReference.FetchAttributesAsync().ConfigureAwait(false);
                var data = new byte[fileReference.Properties.Length];

                cancellationToken.ThrowIfCancellationRequested();
                progress.ReportDownloadFile(fileDirectory.Uri.LocalPath, fileName, data.Length);

                await fileReference.DownloadToByteArrayAsync(data, 0, AccessCondition.GenerateEmptyCondition(),
                    new FileRequestOptions(), new OperationContext(), cancellationToken).ConfigureAwait(false);
                return data;
            }

            return new byte[] { };
        }

        public async Task UploadFileAsync<T>(
            byte[] data,
            string fileName,
            T directory,
            IProgress<StorageProgressDto> progress,
            CancellationToken cancellationToken)
        {
            var fileDirectory = directory as CloudFileDirectory;

            if (fileDirectory != null)
            {
                var fileReference = fileDirectory.GetFileReference(fileName);

                cancellationToken.ThrowIfCancellationRequested();
                progress.ReportUploadFile(fileReference.Uri.LocalPath, data.Length);

                await fileReference.UploadFromByteArrayAsync(data, 0, data.Length,
                    AccessCondition.GenerateEmptyCondition(),
                    new FileRequestOptions(), new OperationContext(), cancellationToken);
            }
        }
    }
}