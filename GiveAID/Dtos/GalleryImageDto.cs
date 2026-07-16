using System.ComponentModel.DataAnnotations;

namespace GiveAID.Dtos;

public record GalleryImageSaveDto(
    [property: Required(ErrorMessage = "Image URI is required.")]
    string ImageUri,

    [property: MaxLength(255, ErrorMessage = "Caption cannot exceed 255 characters.")]
    string? Caption,

    Guid? AssociatedProgrammeId
);

public record GalleryImageDto(Guid Id, string ImageUri, string Caption, (Guid Id, string Name)? AssociatedProgramme);
