// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//   
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Worker
{
    using System;
    using System.Threading;

    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Auth;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.Queue;

    /// <summary>
    /// The program.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        private static void Main(string[] args)
        {
            var credentials = new StorageCredentials("[Enter your account name]", "[Enter your account key]");

            // Retrieve storage account from connection string.
            var storageAccount = new CloudStorageAccount(credentials, true);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue
            CloudQueue queue = queueClient.GetQueueReference("dataqueue");

            Console.WriteLine("Up and listening to the queue.");

            while (true)
            {
                CloudQueueMessage message;
                do
                {
                    message = queue.GetMessage();
                    if (message != null)
                    {
                        Console.WriteLine("Received Message for BLOB " + message.AsString);
                        var blobUrl = message.AsString;
                        var blockBlob = new CloudBlockBlob(new Uri(blobUrl), credentials);

                        // TODO: download and process BLOB 
                        /*using (var fileStream = System.IO.File.OpenWrite(@"path\myfile"))
                        {
                            blockBlob.DownloadToStream(fileStream);
                        }*/
                        if (blockBlob.Exists())
                        {
                            blockBlob.Delete();
                        }

                        queue.DeleteMessage(message);
                        Console.WriteLine("BLOB " + message.AsString + " and queue message deleted.");
                    }
                }
                while (message != null);
    
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }
    }
}