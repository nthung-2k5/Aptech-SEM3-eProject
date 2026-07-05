namespace GiveAID.Dtos;

public record AboutUsSubpageSummaryDto(string Slug, string Title);

public record AboutUsSubpageDetailsDto(string Slug, string Title, string HtmlContent): AboutUsSubpageSummaryDto(Slug, Title);
