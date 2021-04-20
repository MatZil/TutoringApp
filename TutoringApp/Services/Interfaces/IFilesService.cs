using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace TutoringApp.Services.Interfaces
{
    public interface IFilesService
    {
        Task UploadFile(IFormFile formFile, string path);
        Stream DownloadFile(string path);
        void CreateDirectory(string path);
        void DeleteFile(string path);
    }
}
