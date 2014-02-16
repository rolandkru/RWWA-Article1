// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//   
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Client
{
    using System;
    using System.IO;
    using System.Net.Http;

    using Common;

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
            Console.WriteLine("Hello!");

            string command;
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:10281/");

            do
            {
                string fileName = string.Empty;
                while (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
                {
                    Console.Write("Enter the full name of the file you like to send to server: ");
                    fileName = Console.ReadLine();
                }

                var fileInfo = new FileInfo(fileName);

                Console.WriteLine("Asking Server for SAS-URLs");
                HttpResponseMessage resp = client.GetAsync("api/Data").Result;
                resp.EnsureSuccessStatusCode();
                var dataDto = resp.Content.ReadAsAsync<DataDto>().Result;

                Console.WriteLine("Server responds:");
                Console.WriteLine("BLOB Container URL: " + dataDto.BlobContainerUrl);
                Console.WriteLine("BLOB Container SAS Token: " + dataDto.BlobSasToken);
                Console.WriteLine("Queue URL: " + dataDto.QueueUrl);
                Console.WriteLine("Queue SAS Token: " + dataDto.QueueSasToken);

                Console.WriteLine("Create or overwrite the blob with contents from a local file...");
                var container = new CloudBlobContainer(new Uri(dataDto.BlobContainerUrl + dataDto.BlobSasToken));
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileInfo.Name);
                using (var fileStream = File.OpenRead(fileInfo.FullName))
                {
                    blockBlob.UploadFromStream(fileStream);
                }

                Console.WriteLine("done.");

                Console.Write("Add a new message to the queue...");
                var queue = new CloudQueue(new Uri(dataDto.QueueUrl + dataDto.QueueSasToken));
                var message = new CloudQueueMessage(blockBlob.Uri.ToString());
                queue.AddMessage(message);
                Console.WriteLine("done.");
                
                Console.WriteLine("Press Enter to upload the next file or type 'quit' for exit");
                command = Console.ReadLine();
            }
            while (command != "quit");
        }
    }
}