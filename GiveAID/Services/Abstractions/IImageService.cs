using Microsoft.AspNetCore.Http;

namespace GiveAID.Services.Abstractions;

public interface IImageService
{
    Task<string> UploadImageAsync(string folder, string extension, byte[] fileBytes);
    Task<string> UpdateImageAsync(string imageUrl, byte[] fileBytes);
    Task DeleteImageAsync(string imageUrl);
}
