using GiveAID.Dtos;

namespace GiveAID.Models;

public class MockData
{
    public static MemberDto[] Members { get; set; } = [
        new(Guid.NewGuid(), "John Doe", "john.doe@example.com", "password123", new DateOnly(1990, 1, 1), "123 Main St", "1234567890", "Software Engineer"),
        new(Guid.NewGuid(), "Jane Smith", "jane.smith@example.com", "password456", new DateOnly(1985, 5, 15), "456 Elm St", "0987654321", "Doctor"),
        new(Guid.NewGuid(), "Alice Johnson", "alice.j@example.com", "password789", new DateOnly(1992, 8, 20), "789 Oak St", "1112223333", "Teacher")
    ];

    public static NgoDto[] Ngos { get; set; } = [
        new(Guid.NewGuid(), "Global Helpers", "An NGO dedicated to global health.", "100 Health Way", "1-800-HELP", "https://globalhelpers.org"),
        new(Guid.NewGuid(), "Education First", "Providing education to children worldwide.", "200 School Dr", "1-800-EDU", "https://educationfirst.org"),
        new(Guid.NewGuid(), "Green Earth", "Environmental protection and conservation.", "300 Nature Ln", "1-800-GREEN", "https://greenearth.org")
    ];

    public static ProgrammeDto[] Programmes { get; set; } = [
        new(Guid.NewGuid(), "Clean Water Initiative", "Clean Water", "Global Helpers", "https://images.unsplash.com/photo-1516905542749-d3e91122d250?q=80&w=2070&auto=format&fit=crop", DateTime.Now.AddDays(-30), DateTime.Now.AddDays(30), 150, 10000m, 5000m, "Providing clean water to communities in need.", "Global Helpers Team"),
        new(Guid.NewGuid(), "Build a School", "Education", "Education First", "https://images.unsplash.com/photo-1509062522246-3755977927d7?q=80&w=2132&auto=format&fit=crop", DateTime.Now.AddDays(-60), DateTime.Now.AddDays(60), 300, 50000m, 25000m, "Building schools in rural areas.", "Education First Team"),
        new(Guid.NewGuid(), "Plant a Million Trees", "Environment", "Green Earth", "https://images.unsplash.com/photo-1542601906990-b4d3fb778b09?q=80&w=2113&auto=format&fit=crop", DateTime.Now.AddDays(-10), DateTime.Now.AddDays(100), 50, 20000m, 1000m, "Reforestation project to combat climate change.", "Green Earth Team")
    ];

    public static UserQueryDto[] Queries { get; set; } = [
        new(Guid.NewGuid(), Members[0].Id, Members[0].FullName, "Donation Issue", "I am having trouble making a donation.", "Please try clearing your browser cache.", DateTimeOffset.Now.AddDays(-2)),
        new(Guid.NewGuid(), Members[1].Id, Members[1].FullName, "Volunteer Inquiry", "How can I volunteer for the upcoming event?", "We will send you a volunteer form shortly.", DateTimeOffset.Now.AddDays(-1)),
        new(Guid.NewGuid(), Members[2].Id, Members[2].FullName, "Tax Receipt", "When will I receive my tax receipt?", null, DateTimeOffset.Now)
    ];

    public static DonationDto[] Donations { get; set; } = [
        new(Guid.NewGuid(), Members[0].Id, Members[0].FullName, new DonateForProgrammeTarget(Programmes[0].Id), 100m, DateTimeOffset.Now.AddDays(-5)),
        new(Guid.NewGuid(), Members[1].Id, Members[1].FullName, new DonateForNgoTarget(Ngos[1].Id, Guid.NewGuid()), 50m, DateTimeOffset.Now.AddDays(-3)),
        new(Guid.NewGuid(), Members[2].Id, Members[2].FullName, new DonateForProgrammeTarget(Programmes[2].Id), 25m, DateTimeOffset.Now.AddDays(-1))
    ];

    public static GalleryImageDto[] GalleryImages { get; set; } = [
        new GalleryImageDto(Guid.NewGuid(), new Uri("https://images.unsplash.com/photo-1488521787991-ed7bbaae773c?q=80&w=2070&auto=format&fit=crop"), "Children smiling", (Programmes[1].Id, Programmes[1].Name)),
        new GalleryImageDto(Guid.NewGuid(), new Uri("https://images.unsplash.com/photo-1536856136534-bb679c52a9aa?q=80&w=2070&auto=format&fit=crop"), "New well installed", (Programmes[0].Id, Programmes[0].Name)),
        new GalleryImageDto(Guid.NewGuid(), new Uri("https://images.unsplash.com/photo-1542601906990-b4d3fb778b09?q=80&w=2113&auto=format&fit=crop"), "Tree planting event", (Programmes[2].Id, Programmes[2].Name))
    ];
}
