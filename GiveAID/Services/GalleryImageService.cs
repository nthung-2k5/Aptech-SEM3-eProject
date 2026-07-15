using GiveAID.Data;
using GiveAID.Dtos;
using GiveAID.Models;
using GiveAID.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Services;

public class GalleryImageService(AppDbContext dbContext, IImageService imageService) : IGalleryImageService
{
    public async Task<GalleryImageDto[]> GetAllImagesAsync(CancellationToken ct = default)
    {
        // Two-step: SQL projection first (Uri cannot be translated to SQL)
        var rows = await dbContext.GalleryImages.AsNoTracking().OrderByDescending(i => i.ImageId).Select(i => new
        {
            i.ImageId,
            i.ImageUrl,
            i.Caption,
            Programme = i.Programme != null ? new { i.Programme.ProgrammeId, i.Programme.Name } : null
        }).ToArrayAsync(ct);

        return rows.Select(i => new GalleryImageDto(
            i.ImageId,
            i.ImageUrl,
            i.Caption ?? string.Empty,
            i.Programme != null ? (i.Programme.ProgrammeId, i.Programme.Name) : null)).ToArray();
    }

    public async Task<GalleryImageDto?> GetImageByIdAsync(Guid id, CancellationToken ct = default)
    {
        var row = await dbContext.GalleryImages.AsNoTracking().Where(i => i.ImageId == id).Select(i => new
        {
            i.ImageId,
            i.ImageUrl,
            i.Caption,
            Programme = i.Programme != null ? new { i.Programme.ProgrammeId, i.Programme.Name } : null
        }).FirstOrDefaultAsync(ct);

        if (row == null) { return null; }

        return new GalleryImageDto(
            row.ImageId,
            row.ImageUrl,
            row.Caption ?? string.Empty,
            row.Programme != null ? (row.Programme.ProgrammeId, row.Programme.Name) : null);
    }

    public async Task<bool> UploadImageAsync(GalleryImageSaveDto image, CancellationToken ct = default)
    {
        // Validate FK: programme must exist and not be soft-deleted
        if (image.AssociatedProgrammeId.HasValue)
        {
            bool progExists = await dbContext.WelfareProgrammes.AnyAsync(
                p => p.ProgrammeId == image.AssociatedProgrammeId.Value && !p.IsDeleted,
                ct);

            if (!progExists) { return false; }
        }

        var entity = new GalleryImage
        {
            ImageUrl = image.ImageUri,
            Caption = image.Caption,
            ProgrammeId = image.AssociatedProgrammeId
        };

        dbContext.GalleryImages.Add(entity);
        return await dbContext.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateImageAsync(Guid id, GalleryImageSaveDto image, CancellationToken ct = default)
    {
        // Validate FK: programme must exist and not be soft-deleted
        if (image.AssociatedProgrammeId.HasValue)
        {
            bool progExists = await dbContext.WelfareProgrammes.AnyAsync(
                p => p.ProgrammeId == image.AssociatedProgrammeId.Value && !p.IsDeleted,
                ct);

            if (!progExists) { return false; }
        }

        var entity = await dbContext.GalleryImages.FirstOrDefaultAsync(i => i.ImageId == id, ct);

        if (entity == null) { return false; }

        entity.ImageUrl = image.ImageUri;
        entity.Caption = image.Caption;
        entity.ProgrammeId = image.AssociatedProgrammeId;

        await dbContext.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteImageAsync(Guid id, CancellationToken ct = default)
    {
        string? imageUrl = await dbContext.GalleryImages.AsNoTracking().Where(i => i.ImageId == id).Select(i => i.ImageUrl).FirstOrDefaultAsync(ct);

        if (!string.IsNullOrEmpty(imageUrl)) { await imageService.DeleteImageAsync(imageUrl); }

        // Hard delete — GalleryImage has no IsDeleted field
        return await dbContext.GalleryImages.Where(i => i.ImageId == id).ExecuteDeleteAsync(ct) > 0;
    }

    /// <summary>
    /// Safely parses a URL string into a Uri.
    /// Returns a fallback Uri if the stored string is malformed or empty.
    /// </summary>
    private static Uri ParseUri(string url) =>
            Uri.TryCreate(url, UriKind.Absolute, out var uri) ? uri : new Uri("about:blank");
}
