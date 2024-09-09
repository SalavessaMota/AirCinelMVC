using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AirCinelMVC.Helpers
{
    public interface IImageHelper
    {
        Task<string> UploadImageAsync(IFormFile imageFile, string folder);
    }
}
