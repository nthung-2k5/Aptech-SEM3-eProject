using GiveAID.Services.Abstractions;

namespace GiveAID.Services;

public class ImageService(IWebHostEnvironment environment) : IImageService
{
    public async Task<string> UploadImageAsync(string folder, string extension, byte[] bytes)
    {
        if (bytes.Length == 0)
        {
            return string.Empty;
        }

        try
        {
            string uploadsFolder = Path.Combine(environment.WebRootPath, "images", folder);
            
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string filename = Path.GetRandomFileName() + extension;

            string filePath = Path.Combine(uploadsFolder, filename);

            await File.WriteAllBytesAsync(filePath, bytes);

            return $"/images/{folder}/{filename}";
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }
    
    private static (string folder, string filename) ExtractImageUrlPath(string url)
    {
        url = url.TrimStart('/');
        string[] split = url.Split('/');
        return (split[1], split[2]);
    }
    
    public async Task<string> UpdateImageAsync(string imageUrl, byte[] fileBytes)
    {
        (string folder, string filename) = ExtractImageUrlPath(imageUrl);
        await DeleteImageAsync(imageUrl);
        return await UploadImageAsync(folder, Path.GetExtension(filename), fileBytes);
    }

    public Task DeleteImageAsync(string imageUrl)
    {
        (string folder, string filename) = ExtractImageUrlPath(imageUrl);
        string filePath = Path.Combine(environment.WebRootPath, "images", folder, filename);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        return Task.CompletedTask;
    }
}
