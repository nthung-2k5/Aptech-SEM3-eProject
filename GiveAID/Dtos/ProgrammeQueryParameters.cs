namespace GiveAID.Dtos;

public class ProgrammeQueryParameters
{
    // Search
    public string? SearchTerm { get; set; }

    // Filter
    public string? Ngo { get; set; }
    public string? Cause { get; set; }

    // Pagination
    private const int MaxPageSize = 50;
    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get;
        set => field = value > MaxPageSize ? MaxPageSize : value;
    }
}
