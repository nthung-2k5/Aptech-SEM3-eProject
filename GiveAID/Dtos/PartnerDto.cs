using System;
using System.ComponentModel.DataAnnotations;

namespace GiveAID.Dtos;

public record PartnerSaveDto(
    [property: Required(ErrorMessage = "Partner name is required.")]
    [property: MaxLength(200, ErrorMessage = "Partner name cannot exceed 200 characters.")]
    string Name,

    [property: Required(ErrorMessage = "Logo URL is required.")]
    [property: Url(ErrorMessage = "Invalid URL format for Logo.")]
    string LogoUrl,

    [property: Required(ErrorMessage = "Description is required.")]
    string Description,

    [property: Required(ErrorMessage = "Website link is required.")]
    [property: Url(ErrorMessage = "Invalid URL format for Website.")]
    string WebsiteLink
);

public record PartnerSummaryDto(Guid Id, string Name, string LogoUrl);

public record PartnerDto(Guid Id, string Name, string LogoUrl, string Description, string WebsiteLink)
    : PartnerSummaryDto(Id, Name, LogoUrl);