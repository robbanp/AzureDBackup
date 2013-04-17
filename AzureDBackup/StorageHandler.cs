//By Robert Pohl, robert@sugarcubesolutions.com

using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureDBackup
{
    public class StorageHandler
    {
        /// <summary>
        ///     Create the storage handler connected to the storage info
        /// </summary>
        /// <param name="storageInfo">Connection to storage account</param>
        public StorageHandler(string storageInfo)
        {
            storageAccount = CloudStorageAccount.Parse(storageInfo);
        }

        private CloudStorageAccount storageAccount { get; set; }

        /// <summary>
        ///     Get blob reference
        /// </summary>
        /// <param name="container">Container name</param>
        /// <param name="fileName">Blob file name</param>
        /// <returns></returns>
        public CloudBlockBlob GetBlob(string container, string fileName)
        {
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(container);
            return blobContainer.GetBlockBlobReference(fileName);
        }

        /// <summary>
        ///     Put local file to blob storage
        /// </summary>
        /// <param name="container">Container name</param>
        /// <param name="fileName">Path to local file</param>
        public void SetBlob(string container, string fileName)
        {
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(container);
            blobContainer.CreateIfNotExists();
            blobContainer.SetPermissions(
                new BlobContainerPermissions
                    {
                        PublicAccess =
                            BlobContainerPublicAccessType.Blob
                    });

            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(Path.GetFileName(fileName));
            using (FileStream fileStream = File.OpenRead(fileName))
            {
                blockBlob.UploadFromStream(fileStream);
            }
        }
    }
}