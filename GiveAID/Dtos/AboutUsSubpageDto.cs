using GiveAID.Models;

namespace GiveAID.Dtos;

public record AboutUsSubpageSummaryDto(string Slug, string Title);

public record AboutUsSubpageDto(string Slug, string Title, string HtmlContent)
        : AboutUsSubpageSummaryDto(Slug, Title);

public static class AboutUsSubpageMapper
{
    extension(AboutUsSubpage subpage)
    {
        public AboutUsSubpageSummaryDto MapToSummaryDto() => new(subpage.Slug, subpage.Title);

        public AboutUsSubpageDto MapToDto() => new(subpage.Slug, subpage.Title, subpage.HtmlContent);
    }

    extension(AboutUsSubpageDto dto)
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

        public IQueryable<AboutUsSubpageDto> ProjectToDto() =>
                subpages.Select(subpage =>
                        new AboutUsSubpageDto(subpage.Slug, subpage.Title, subpage.HtmlContent));
    }
}
