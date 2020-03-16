using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace azure_academy.Shared
{
    public class LocalFileSystem : IFileSystem
    {
        private readonly IWebHostEnvironment env = null;
    
        public LocalFileSystem(IWebHostEnvironment env)
        {
            this.env = env;
        }
    
        public string GetRootPath() 
        {
            return string.Empty;
        }
    
        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(Path.Combine(env.WebRootPath, path));
        }
    
        public bool DirectoryExists(string path)
        {
            return Directory.Exists(Path.Combine(env.WebRootPath, path));
        }
    
        public string PathCombine(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }
    
        public async Task SaveFile(IFormFile file, string filePath)
        {
            using (var fileStream = new FileStream(Path.Combine(env.WebRootPath, filePath), FileMode.Create)) {
                await file.CopyToAsync(fileStream);
            }
        }
    }    
}