using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using StorageClient.Azure.Test.Helpers;
using StorageClient.Core.Progress;
using StorageClient.Core.Settings;

namespace StorageClient.Azure.Test
{
    [TestFixture]
    public class AzureStorageClientTest
    {
        [Test]
        [Category("LongRunning")]
        public async Task ShouldDownloadBigFileFromAzure()
        {
            var path = DirectoryHelpers.GetNewTestFolder()[0];

            var settings = new StorageSettings {ConnectionString = Constants.ConnectionString};
            var progress = new Progress<StorageProgressDto>();

            var azureStorageClient = AzureStorageClientBuilder.Build(settings);
            await azureStorageClient.DownloadDirectoryAsync(path, Constants.AzureFileShareWithBigFile, progress);

            var fileEntries = Directory.GetFileSystemEntries(path, "*.*", SearchOption.AllDirectories);
            var directories = fileEntries
                .Where(fileEntry => File.GetAttributes(fileEntry).HasFlag(FileAttributes.Directory)).ToList();
            var files = fileEntries.Except(directories).ToList();

            Assert.AreEqual(1, fileEntries.Length);
            Assert.AreEqual(0, directories.Count);
            Assert.AreEqual(1, files.Count);

            var fileInfo = new FileInfo(files[0]);
            Assert.AreEqual(52428800, fileInfo.Length);
            Assert.AreEqual("bigFile.dat", fileInfo.Name);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task ShouldDownloadDirectoryFromAzure()
        {
            var path = DirectoryHelpers.GetNewTestFolder()[0];

            var settings = new StorageSettings {ConnectionString = Constants.ConnectionString};
            var progress = new Progress<StorageProgressDto>();

            var azureStorageClient = AzureStorageClientBuilder.Build(settings);
            await azureStorageClient.DownloadDirectoryAsync(path, Constants.AzureFileShareWithStructure, progress);

            var fileEntries = Directory.GetFileSystemEntries(path, "*.*", SearchOption.AllDirectories);

            var directories = fileEntries
                .Where(fileEntry => File.GetAttributes(fileEntry).HasFlag(FileAttributes.Directory)).ToList();
            var files = fileEntries.Except(directories).ToList();

            Assert.IsTrue(Directory.Exists(path));

            Assert.AreEqual(14, fileEntries.Length);
            Assert.AreEqual(6, directories.Count);
            Assert.AreEqual(8, files.Count);

            Assert.IsTrue(files.Contains($"{path}\\Dir1\\Dir1a\\file2InDir1a.txt"));
            Assert.IsTrue(files.Contains($"{path}\\Dir1\\Dir1a\\fileInDir1a.txt"));
            Assert.IsTrue(files.Contains($"{path}\\Dir1\\Dir1b\\fileI2nDir1b.txt"));
            Assert.IsTrue(files.Contains($"{path}\\Dir1\\Dir1b\\fileInDir1b.txt"));
            Assert.IsTrue(files.Contains($"{path}\\Dir1\\fileInDir1.txt"));
            Assert.IsTrue(files.Contains($"{path}\\Dir2\\fileInDir2.txt"));
            Assert.IsTrue(files.Contains($"{path}\\Dir2\\Dir2a\\fileInDir2a.txt"));
            Assert.IsTrue(files.Contains($"{path}\\Dir2\\Dir2b\\fileInDir2b.txt"));

            Assert.IsTrue(directories.Contains($"{path}\\Dir1\\Dir1b"));
            Assert.IsTrue(directories.Contains($"{path}\\Dir1\\Dir1b"));
            Assert.IsTrue(directories.Contains($"{path}\\Dir1"));
            Assert.IsTrue(directories.Contains($"{path}\\Dir2"));
            Assert.IsTrue(directories.Contains($"{path}\\Dir2\\Dir2a"));
            Assert.IsTrue(directories.Contains($"{path}\\Dir2\\Dir2b"));

            Assert.IsTrue(new FileInfo($"{path}\\Dir1\\Dir1a\\file2InDir1a.txt").Length > 0);
            Assert.IsTrue(new FileInfo($"{path}\\Dir1\\Dir1a\\fileInDir1a.txt").Length > 0);
            Assert.IsTrue(new FileInfo($"{path}\\Dir1\\Dir1b\\fileI2nDir1b.txt").Length > 0);
            Assert.IsTrue(new FileInfo($"{path}\\Dir1\\Dir1b\\fileInDir1b.txt").Length > 0);
            Assert.IsTrue(new FileInfo($"{path}\\Dir1\\fileInDir1.txt").Length > 0);
            Assert.IsTrue(new FileInfo($"{path}\\Dir2\\fileInDir2.txt").Length > 0);
            Assert.IsTrue(new FileInfo($"{path}\\Dir2\\Dir2a\\fileInDir2a.txt").Length > 0);
            Assert.IsTrue(new FileInfo($"{path}\\Dir2\\Dir2b\\fileInDir2b.txt").Length > 0);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task ShouldDownloadFileFromAzure()
        {
            var localPath = @"F:\Test\06102018220836329\0";
            var storagePath = "bigfile/Dir2/Dir2b/fileInDir2b.txt";
            var settings = new StorageSettings {ConnectionString = Constants.ConnectionString};
            var progress = new Progress<StorageProgressDto>();

            var azureStorageClient = AzureStorageClientBuilder.Build(settings);
            await azureStorageClient.DownloadFileAsync(localPath, storagePath, progress);

            Assert.IsTrue(File.Exists($@"{localPath}\fileInDir2b.txt"));
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task ShouldDownloadSubDirectoryFromAzure()
        {
            var path = DirectoryHelpers.GetNewTestFolder()[0];

            var settings = new StorageSettings {ConnectionString = Constants.ConnectionString};
            var progress = new Progress<StorageProgressDto>();

            var azureStorageClient = AzureStorageClientBuilder.Build(settings);
            await azureStorageClient.DownloadDirectoryAsync(path, $"{Constants.AzureFileShareWithStructure}/Dir1",
                progress);

            var fileEntries = Directory.GetFileSystemEntries(path, "*.*", SearchOption.AllDirectories);

            var directories = fileEntries
                .Where(fileEntry => File.GetAttributes(fileEntry).HasFlag(FileAttributes.Directory)).ToList();
            var files = fileEntries.Except(directories).ToList();

            Assert.IsTrue(Directory.Exists(path));

            Assert.AreEqual(7, fileEntries.Length);
            Assert.AreEqual(2, directories.Count);
            Assert.AreEqual(5, files.Count);

            Assert.IsTrue(files.Contains($"{path}\\Dir1a\\file2InDir1a.txt"));
            Assert.IsTrue(files.Contains($"{path}\\Dir1a\\fileInDir1a.txt"));
            Assert.IsTrue(files.Contains($"{path}\\Dir1b\\fileI2nDir1b.txt"));
            Assert.IsTrue(files.Contains($"{path}\\Dir1b\\fileInDir1b.txt"));
            Assert.IsTrue(files.Contains($"{path}\\fileInDir1.txt"));

            Assert.IsTrue(directories.Contains($"{path}\\Dir1a"));
            Assert.IsTrue(directories.Contains($"{path}\\Dir1b"));

            Assert.IsTrue(new FileInfo($"{path}\\Dir1a\\file2InDir1a.txt").Length > 0);
            Assert.IsTrue(new FileInfo($"{path}\\Dir1a\\fileInDir1a.txt").Length > 0);
            Assert.IsTrue(new FileInfo($"{path}\\Dir1b\\fileI2nDir1b.txt").Length > 0);
            Assert.IsTrue(new FileInfo($"{path}\\Dir1b\\fileInDir1b.txt").Length > 0);
            Assert.IsTrue(new FileInfo($"{path}\\fileInDir1.txt").Length > 0);
        }

        [Test]
        [Category("IntegrationTest")]
        public void ShouldReportDuringCancelIsRequested()
        {
            var events = new List<StorageProgressDto>();
            var settings = new StorageSettings {ConnectionString = Constants.ConnectionString};
            var azureStorageClient = AzureStorageClientBuilder.Build(settings);
            var localPath = $"F:\\Test\\{DateTime.Now:hhmmsst}";
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            var progress = new Progress<StorageProgressDto>();
            progress.ProgressChanged += (s, e) => { events.Add(e); };

            Assert.ThrowsAsync<TaskCanceledException>(async () =>
                await azureStorageClient.DownloadDirectoryAsync(localPath, "sth",
                    progress, cancellationTokenSource.Token));

            Assert.AreEqual(1, events.Count);
            Assert.AreEqual(StorageProgressJobType.CleanDirectory, events[0].StorageProgressJobType);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task ShouldReportDuringDownloading()
        {
            var events = new List<StorageProgressDto>();
            var path = DirectoryHelpers.GetNewTestFolder()[0];

            var settings = new StorageSettings {ConnectionString = Constants.ConnectionString};
            var progress = new Progress<StorageProgressDto>();
            progress.ProgressChanged += (s, e) => { events.Add(e); };

            var azureStorageClient = AzureStorageClientBuilder.Build(settings);
            await azureStorageClient.DownloadDirectoryAsync(path, Constants.AzureFileShareWithStructure, progress);

            Assert.AreEqual(38, events.Count);
        }

        [Test]
        [Category("IntegrationTest")]
        public void ShouldThrowExceptionIfCancelIsRequestedDuringDownloading()
        {
            var settings = new StorageSettings {ConnectionString = Constants.ConnectionString};
            var azureStorageClient = AzureStorageClientBuilder.Build(settings);
            var progress = new Progress<StorageProgressDto>();
            var localPath = $"F:\\Test\\{DateTime.Now:hhmmsst}";
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            Assert.ThrowsAsync<TaskCanceledException>(async () =>
                await azureStorageClient.DownloadDirectoryAsync(localPath, "sth", progress,
                    cancellationTokenSource.Token));
        }

        [Test]
        [Category("IntegrationTest")]
        public void ShouldThrowExceptionIfCancelIsRequestedDuringUploading()
        {
            var settings = new StorageSettings {ConnectionString = Constants.ConnectionString};
            var azureStorageClient = AzureStorageClientBuilder.Build(settings);
            var path = @"F:\Test\06102018220836329\0";
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            Assert.ThrowsAsync<TaskCanceledException>(async () =>
                await azureStorageClient.UploadDirectoryAsync(path, Constants.AzureFileShareWithBigFile,
                    cancellationToken: cancellationTokenSource.Token));
        }

        [Test]
        [Category("IntegrationTest")]
        public void ShouldThrowExceptionIfDirectoryIsNotEmptyDuringDownloading()
        {
            var settings = new StorageSettings {ConnectionString = Constants.ConnectionString};
            var azureStorageClient = AzureStorageClientBuilder.Build(settings);
            var localPath = "F:\\Test";

            var exception = Assert.ThrowsAsync<IOException>(async () =>
                await azureStorageClient.DownloadDirectoryAsync(localPath, "sth"));
            Assert.AreEqual($"The directory '{localPath}' is not empty.", exception.Message);
        }

        [Test]
        [Category("IntegrationTest")]
        public void ShouldThrowExceptionIfLocalPathIsEmptyDuringDownloading()
        {
            var settings = new StorageSettings {ConnectionString = Constants.ConnectionString};
            var azureStorageClient = AzureStorageClientBuilder.Build(settings);

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await azureStorageClient.DownloadDirectoryAsync("", ""));
        }

        [Test]
        [Category("IntegrationTest")]
        public void ShouldThrowExceptionIfLocalPathIsEmptyDuringUploading()
        {
            var settings = new StorageSettings {ConnectionString = Constants.ConnectionString};

            var azureStorageClient = AzureStorageClientBuilder.Build(settings);

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await azureStorageClient.UploadDirectoryAsync(string.Empty, Constants.AzureFileShareWithBigFile));
        }

        [Test]
        [Category("IntegrationTest")]
        public void ShouldThrowExceptionIfStoragePathIsEmptyDuringDownloading()
        {
            var settings = new StorageSettings {ConnectionString = Constants.ConnectionString};
            var azureStorageClient = AzureStorageClientBuilder.Build(settings);

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await azureStorageClient.DownloadDirectoryAsync("C:\\", string.Empty));
        }

        [Test]
        [Category("IntegrationTest")]
        public void ShouldThrowExceptionIfStoragePathIsEmptyDuringUploading()
        {
            var path = @"F:\Test\06102018220836329\0";
            var settings = new StorageSettings {ConnectionString = Constants.ConnectionString};

            var azureStorageClient = AzureStorageClientBuilder.Build(settings);

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await azureStorageClient.UploadDirectoryAsync(path, string.Empty));
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task ShouldUploadDirectoryToAzure()
        {
            var path = @"F:\Test\06102018220836329\0";
            var settings = new StorageSettings {ConnectionString = Constants.ConnectionString};
            var progress = new Progress<StorageProgressDto>();

            var azureStorageClient = AzureStorageClientBuilder.Build(settings);
            await azureStorageClient.UploadDirectoryAsync(path, Constants.AzureFileShareWithBigFile, progress);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task ShouldUploadFileToAzure()
        {
            var localPath = @"F:\Test\06102018220836329\0\fileInDir2b.txt";
            var storagePath = "bigfile/1/2/fileInDir2b.txt";
            var settings = new StorageSettings {ConnectionString = Constants.ConnectionString};
            var progress = new Progress<StorageProgressDto>();

            var azureStorageClient = AzureStorageClientBuilder.Build(settings);
            await azureStorageClient.UploadFileAsync(localPath, storagePath, progress);
        }
    }
}