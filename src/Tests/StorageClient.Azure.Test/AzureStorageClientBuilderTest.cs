using NUnit.Framework;
using StorageClient.Core.Settings;

namespace StorageClient.Azure.Test
{
    [TestFixture]
    public class AzureStorageClientBuilderTest
    {
        [Test]
        public void ShouldBuildAzureStorageClientAndReturnInstance()
        {
            var settings = new StorageSettings {ConnectionString = Constants.ConnectionString};
            var azureStorageClient = AzureStorageClientBuilder.Build(settings);

            Assert.IsNotNull(azureStorageClient);
        }
    }
}