// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataDto.cs" company="">
//   
// </copyright>
// <summary>
//   The data DTO.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Common
{
    using System;

    /// <summary>
    ///     The data DTO.
    /// </summary>
    public class DataDto
    {
        /// <summary>
        /// Gets or sets the blob url.
        /// </summary>
        public Uri BlobContainerUrl { get; set; }

        /// <summary>
        /// Gets or sets the blob SAS token.
        /// </summary>
        public string BlobSasToken { get; set; }

        /// <summary>
        /// Gets or sets the queue url.
        /// </summary>
        public Uri QueueUrl { get; set; }

        /// <summary>
        /// Gets or sets the queue SAS token.
        /// </summary>
        public string QueueSasToken { get; set; }
    }
}