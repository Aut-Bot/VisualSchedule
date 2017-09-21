using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types
using Microsoft.Azure;

namespace AzureStorageApp
{
    class Program
    {
        static void Main(string[] args)
        {
            SaveStorage();
            Console.ReadKey();
        }

        private static void SaveStorage()
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("photo");

            //Create a new container, if it does not exist
            var exists = container.CreateIfNotExists();

            // Retrieve reference to a blob named "myblob".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("pg");

            //// OPTIONAL: set public permissions
            //container.SetPermissions(
            //    new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            
            // Create or overwrite the "myblob" blob with contents from a local file.
            using (var fileStream = System.IO.File.OpenRead(Environment.CurrentDirectory + "\\pg.jpg"))
            {
                blockBlob.UploadFromStream(fileStream);
            }

        }
    }
}
