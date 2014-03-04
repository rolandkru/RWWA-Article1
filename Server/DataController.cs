// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataController.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the DataController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RWWA_Article1.Server
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Http;

    using Common;

    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Auth;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.Queue;

    /// <summary>
    /// The values controller.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    public class DataController : ApiController
    {
        // GET api/values        
        public DataDto Get()
        {
            Console.WriteLine("Received a request from a client.");

            var dto = new DataDto();

            var credentials = new StorageCredentials("[Enter your account name]", "[Enter your account key]");

            // Retrieve storage account from connection string.
            var storageAccount = new CloudStorageAccount(credentials, true);

            GenerateBlobContainerSasUrl(storageAccount, dto);
            GenerateQueueSasUrl(storageAccount, dto);

            Console.WriteLine("Respond with Blob and Queue SAS Urls.");
            return dto;
        }

        private static void GenerateBlobContainerSasUrl(CloudStorageAccount storageAccount, DataDto dto)
        {
            // Create the blob client.
            var blobClient = storageAccount.CreateCloudBlobClient();

            // Use the client's name as container
            var container = blobClient.GetContainerReference("datacontainer");

            // Create the container if it doesn't already exist.
            // TODO: Avoid the next line of code in productive systems, since it makes call to azure blob storage. 
            // It's better to make sure the container always exists from the beginning.
            container.CreateIfNotExists();

            // Set the expiry time and permissions for the container. 
            // In this case no start time is specified, so the shared access signature becomes valid immediately.
            var accessBlobPolicy = new SharedAccessBlobPolicy();
            accessBlobPolicy.SharedAccessExpiryTime = DateTime.UtcNow.AddHours(1);
            accessBlobPolicy.Permissions = SharedAccessBlobPermissions.Write;

            // Generate the SAS token. No access policy identifier is used which makes it non revocable.
            // The token is generated without issuing any calls against the Windows Azure Storage.
            string sasToken = container.GetSharedAccessSignature(accessBlobPolicy);

            dto.BlobContainerUrl = container.Uri;
            dto.BlobSasToken = sasToken;
        }

        private static void GenerateQueueSasUrl(CloudStorageAccount storageAccount, DataDto dto)
        {
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue
            CloudQueue queue = queueClient.GetQueueReference("dataqueue");

            // Create the queue if it doesn't already exist
            // TODO: Avoid the next line of code in productive systems, since it makes call to azure queue storage. 
            // It's better to make sure the queue always exists from the beginning.
            queue.CreateIfNotExists();

            var accessQueuePolicy = new SharedAccessQueuePolicy();
            accessQueuePolicy.SharedAccessExpiryTime = DateTime.UtcNow.AddHours(1);
            accessQueuePolicy.Permissions = SharedAccessQueuePermissions.Add;

            // Generate the SAS token. No access policy identifier is used which makes it non revocable.
            // The token is generated without issuing any calls against the Windows Azure Storage.
            string sasToken = queue.GetSharedAccessSignature(accessQueuePolicy, null);

            dto.QueueUrl = queue.Uri;
            dto.QueueSasToken = sasToken;
        }
    }
}
