namespace GiveAID.Dtos;

public record GalleryImageSaveDto(Uri ImageUri, string Caption, Guid? AssociatedProgrammeId);

public record GalleryImageDto(Guid Id, Uri ImageUri, string Caption, string AssociatedProgrammeName);
