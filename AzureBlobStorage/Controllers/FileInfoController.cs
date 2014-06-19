using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureBlobStorage.Controllers
{
    public class FileInfoController : ApiController
    {

        private const string AccountName = "";
        private const string Key = "";

        private static readonly StorageCredentials StorageCredentials = new StorageCredentials(AccountName, Key);

        [HttpPost]
        [Route("api/fileInfo")]
        public IHttpActionResult Post(dynamic fileInfo)
        {
            var account = new CloudStorageAccount(StorageCredentials, false); // should be true when production
            var container = account.CreateCloudBlobClient().GetContainerReference("testcors");

            var policy = new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.Write,
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15)
            };
            
            var sasWrite = container.GetSharedAccessSignature(policy);

            return Ok(new {sasUrlWrite = string.Format("{0}/{1}{2}", container.Uri, Guid.NewGuid(), sasWrite)});
        }
    }
}
