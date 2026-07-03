using GiveAID.Dtos;
using GiveAID.Services.Abstractions;

namespace GiveAID.Services;

public class AboutUsSubpageService : IAboutUsSubpageService
{
    private readonly List<AboutUsSubpageDetailsDto> pages =
    [
        new(
            "what-we-do",
            "What We Do",
            """
                <div class="space-y-6">
                    <h2 class="font-['Playfair_Display',serif] text-4xl font-bold text-[#1C1A17]">What We Do</h2>
                    <p class="font-['Source_Sans_3',sans-serif] text-lg text-[#6B6459] leading-relaxed">Give-AID coordinates fundraising and welfare programme delivery across a network of verified NGO partners.</p>
                    <div class="grid md:grid-cols-2 gap-6">
                        <div class="bg-[#F6F1E9] rounded-xl p-6">
                            <h4 class="font-['Playfair_Display',serif] text-lg font-bold text-[#1C1A17] mb-2">Fund Mobilisation</h4>
                            <p class="font-['Source_Sans_3',sans-serif] text-sm text-[#6B6459]">We channel corporate and individual donations to high-impact causes with full transparency.</p>
                        </div>
                        <div class="bg-[#F6F1E9] rounded-xl p-6">
                            <h4 class="font-['Playfair_Display',serif] text-lg font-bold text-[#1C1A17] mb-2">Programme Management</h4>
                            <p class="font-['Source_Sans_3',sans-serif] text-sm text-[#6B6459]">End-to-end coordination of health, education, and social inclusion programmes.</p>
                        </div>
                    </div>
                </div>
                """),

        new(
            "our-mission",
            "Our Mission",
            """
                <div class="space-y-6">
                    <h2 class="font-['Playfair_Display',serif] text-4xl font-bold text-[#1C1A17]">Our Mission</h2>
                    <div class="border-l-4 border-[#C97C2E] pl-6 py-2">
                        <p class="font-['Playfair_Display',serif] text-2xl italic text-[#1A5C6B]">"To build an equitable world where every person — regardless of circumstance — has access to healthcare, education, and dignity."</p>
                    </div>
                    <p class="font-['Source_Sans_3',sans-serif] text-[#6B6459] leading-relaxed">Our mission drives every decision we make: from which NGOs we partner with, to how donations are allocated, to the events we sponsor and the communities we serve.</p>
                </div>
                """),

        new(
            "our-team",
            "Our Team",
            """
                <div class="space-y-6">
                    <h2 class="font-['Playfair_Display',serif] text-4xl font-bold text-[#1C1A17]">Our Team</h2>
                    <p>Our team comprises dedicated professionals.</p>
                </div>
                """),

        new(
            "career",
            "Career With Us",
            """
                <div class="space-y-6">
                    <h2 class="font-['Playfair_Display',serif] text-4xl font-bold text-[#1C1A17]">Career With Us</h2>
                    <p>Join a passionate team working to change the world.</p>
                </div>
                """)
    ];

    public Task<AboutUsSubpageSummaryDto[]> ListSubpagesAsync(CancellationToken ct = default)
    {
        return Task.FromResult<AboutUsSubpageSummaryDto[]>(pages.ToArray());
    }

    public Task<AboutUsSubpageDetailsDto?> GetBySlugAsync(string slug, CancellationToken ct = default)
    {
        var page = pages.FirstOrDefault(p => p.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(page);
    }

    public Task<bool> AddSubpageAsync(AboutUsSubpageDetailsDto page, CancellationToken ct = default)
    {
        bool exists = pages.Any(p => p.Slug.Equals(page.Slug, StringComparison.OrdinalIgnoreCase));
        
        if (!exists) { pages.Add(page); }
        
        return Task.FromResult(!exists);
    }

    public Task<bool> UpdateSubpageAsync(AboutUsSubpageDetailsDto page, CancellationToken ct = default)
    {
        bool exists = pages.Any(p => p.Slug.Equals(page.Slug, StringComparison.OrdinalIgnoreCase));

        if (exists)
        {
            int index = pages.FindIndex(p => p.Slug.Equals(page.Slug, StringComparison.OrdinalIgnoreCase));
            pages[index] = page;
        }
        
        return Task.FromResult(exists);
    }

    public Task<bool> DeleteSubpageAsync(string slug, CancellationToken ct = default)
    {
        return Task.FromResult(pages.RemoveAll(p => p.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase)) > 0);
    }
}
