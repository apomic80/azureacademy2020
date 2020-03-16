using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace azure_academy.Shared
{
    public interface IFileSystem
    {
        string GetRootPath();
        string PathCombine(string path1, string path2);
        bool DirectoryExists(string path);
        void CreateDirectory(string path);
        Task SaveFile(IFormFile file, string filePath);
    }
}