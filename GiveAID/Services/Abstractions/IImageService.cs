using Microsoft.AspNetCore.Http;

namespace GiveAID.Services.Abstractions;

public interface IImageService
{
    Task EnsureBucketExists();
    Task<string> UploadImageAsync(string folder, string filename, byte[] fileBytes);
    Task<string> UpdateImageAsync(Uri imageUrl, byte[] fileBytes);
    Task DeleteImageAsync(Uri imageUrl);
}
