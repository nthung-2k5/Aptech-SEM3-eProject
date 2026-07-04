using GiveAID.Models;
using GiveAID.Dtos;
using GiveAID.Services.Abstractions;

namespace GiveAID.Services;

public class GalleryImageService : IGalleryImageService
{
    public Task<GalleryImageDto[]> GetAllImagesAsync(CancellationToken ct = default) =>
            Task.FromResult(MockData.GalleryImages.ToArray());

    public Task<GalleryImageDto?> GetImageByIdAsync(Guid id, CancellationToken ct = default) =>
            Task.FromResult(MockData.GalleryImages.FirstOrDefault(i => i.Id == id));

    public Task<bool> UploadImageAsync(GalleryImageSaveDto image, CancellationToken ct = default) =>
            throw new NotImplementedException();

    public Task<bool> UpdateImageAsync(Guid id, GalleryImageSaveDto image, CancellationToken ct = default) =>
            throw new NotImplementedException();

    public Task<bool> DeleteImageAsync(Guid id, CancellationToken ct = default) => throw new NotImplementedException();
}
