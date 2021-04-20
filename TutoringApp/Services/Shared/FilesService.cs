using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Services.Shared
{
    public class FilesService : IFilesService
    {
        public async Task UploadFile(IFormFile formFile, string path)
        {
            await using var stream = new FileStream(path, FileMode.Create);

            await formFile.CopyToAsync(stream);
        }

        public Stream DownloadFile(string path)
        {
            return File.OpenRead(path);
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public void DeleteFile(string path)
        {
            File.Delete(path);
        }
    }
}
