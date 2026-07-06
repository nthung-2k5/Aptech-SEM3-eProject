using GiveAID.Models;

namespace GiveAID.Dtos;

public record AboutUsSubpageSummaryDto(string Slug, string Title);

public record AboutUsSubpageDetailsDto(string Slug, string Title, string HtmlContent)
        : AboutUsSubpageSummaryDto(Slug, Title);

public static class AboutUsSubpageMapper
{
    extension(AboutUsSubpage subpage)
    {
        public AboutUsSubpageSummaryDto MapToSummaryDto() => new(subpage.Slug, subpage.Title);

        public AboutUsSubpageDetailsDto MapToDto() => new(subpage.Slug, subpage.Title, subpage.HtmlContent);
    }

    extension(AboutUsSubpageDetailsDto dto)
    {
        public AboutUsSubpage MapToEntity() =>
                new()
                {
                    Slug = dto.Slug,
                    Title = dto.Title
                };
    }

    extension(IQueryable<AboutUsSubpage> subpages)
    {
        public IQueryable<AboutUsSubpageSummaryDto> ProjectToSummaryDto() =>
                subpages.Select(subpage => new AboutUsSubpageSummaryDto(subpage.Slug, subpage.Title));

        public IQueryable<AboutUsSubpageDetailsDto> ProjectToDto() =>
                subpages.Select(subpage =>
                        new AboutUsSubpageDetailsDto(subpage.Slug, subpage.Title, subpage.HtmlContent));
    }
}
