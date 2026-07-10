using Microsoft.AspNetCore.Http;

namespace GiveAID.Services.Abstractions;

public interface IImageService
{
    Task EnsureBucketExists();
    // Task<Uri> GetImageUriAsync(string key);
    Task<string> UploadImageAsync(string folder, string filename, byte[] fileBytes);
}
