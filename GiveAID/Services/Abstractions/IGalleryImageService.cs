using GiveAID.Dtos;

namespace GiveAID.Services.Abstractions;

public interface IGalleryImageService
{
    Task<GalleryImageDto[]> GetAllImagesAsync(CancellationToken ct = default);
    Task<GalleryImageDto?> GetImageByIdAsync(Guid id, CancellationToken ct = default);
    Task<bool> UploadImageAsync(GalleryImageSaveDto image, CancellationToken ct = default);
    Task<bool> UpdateImageAsync(Guid id, GalleryImageSaveDto image, CancellationToken ct = default);
    Task<bool> DeleteImageAsync(Guid id, CancellationToken ct = default);
}
