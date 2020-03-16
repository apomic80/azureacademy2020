using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace azure_academy.Shared
{
    public class AzureBlobFileSystem : IFileSystem
    {
        private readonly CloudBlobContainer blobContainer;

        public AzureBlobFileSystem(
            string storageAccount,
            string storageKey,
            string containerName)
        {
            var account = new CloudStorageAccount(
                new StorageCredentials(
                    storageAccount,
                    storageKey), true);
        
            CloudBlobClient blobClient = account
                .CreateCloudBlobClient();
        
            this.blobContainer =
                blobClient.GetContainerReference(containerName);
        }

        public void CreateDirectory(string path)
        {
            
        }

        public bool DirectoryExists(string path)
        {
            return true;
        }

        public string GetRootPath()
        {
            return this.blobContainer.StorageUri.PrimaryUri.AbsoluteUri + "/";
        }

        public string PathCombine(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }

        public async Task SaveFile(IFormFile file, string filePath)
        {
            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(filePath);
            await blockBlob.UploadFromStreamAsync(file.OpenReadStream());
        }
    }
}