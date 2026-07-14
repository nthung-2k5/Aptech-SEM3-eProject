using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using GiveAID.Services.Abstractions;

namespace GiveAID.Services;

public class S3ImageService(IWebHostEnvironment environment) : IImageService
{
    public Task EnsureBucketExists()
    {
        return Task.CompletedTask;
    }

    public async Task<string> UploadImageAsync(string folder, string filename, byte[] bytes)
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

            string realFilename = Path.GetRandomFileName() + Path.GetExtension(filename);

            string filePath = Path.Combine(uploadsFolder, realFilename);

            await File.WriteAllBytesAsync(filePath, bytes);

            return $"/images/{folder}/{realFilename}";
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }
    
    private static (string folder, string filename) ExtractImageUrlPath(string url)
    {
        url = url["/images".Length..].TrimStart('/');
        return (url.Split('/')[0], url.Split('/')[1]);
    }
    
    public async Task<string> UpdateImageAsync(Uri imageUrl, byte[] fileBytes)
    {
        (string folder, string filename) = ExtractImageUrlPath(imageUrl.ToString());
        await DeleteImageAsync(imageUrl);
        return await UploadImageAsync(folder, filename, fileBytes);
    }

    public Task DeleteImageAsync(Uri imageUrl)
    {
        (string folder, string filename) = ExtractImageUrlPath(imageUrl.ToString());
        string uploadsFolder = Path.Combine(environment.WebRootPath, "images", folder);
        string filePath = Path.Combine(uploadsFolder, filename);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        return Task.CompletedTask;
    }
}
