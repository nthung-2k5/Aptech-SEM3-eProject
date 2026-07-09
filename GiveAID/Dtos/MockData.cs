namespace GiveAID.Dtos;

public class MockData
{
    public static MemberDto[] Members { get; set; } = [
        new(Guid.NewGuid(), "John Doe", "john.doe@example.com", new DateOnly(1990, 1, 1), "123 Main St", "1234567890", "Software Engineer"),
        new(Guid.NewGuid(), "Jane Smith", "jane.smith@example.com", new DateOnly(1985, 5, 15), "456 Elm St", "0987654321", "Doctor"),
        new(Guid.NewGuid(), "Alice Johnson", "alice.j@example.com", new DateOnly(1992, 8, 20), "789 Oak St", "1112223333", "Teacher")
    ];
    public static UserQueryDto[] Queries { get; set; } = [
        new(Guid.NewGuid(), Members[0].Id, Members[0].FullName, "Donation Issue", "I am having trouble making a donation.", "Please try clearing your browser cache.", DateTimeOffset.Now.AddDays(-2)),
        new(Guid.NewGuid(), Members[1].Id, Members[1].FullName, "Volunteer Inquiry", "How can I volunteer for the upcoming event?", "We will send you a volunteer form shortly.", DateTimeOffset.Now.AddDays(-1)),
        new(Guid.NewGuid(), Members[2].Id, Members[2].FullName, "Tax Receipt", "When will I receive my tax receipt?", null, DateTimeOffset.Now)
    ];
}

