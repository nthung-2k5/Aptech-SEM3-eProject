namespace GiveAID.Dtos;

public class ProgrammeQueryParameters
{
    // Search
    public string? SearchTerm { get; set; }

    // Filter
    public Guid? NgoId { get; set; }
    public Guid? CauseId { get; set; }
    public string? StatusFilter { get; set; } // "Active", "Upcoming", "Ended"

    // Sorting
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; } = true;

    // Pagination
    private const int MaxPageSize = 50;
    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get;
        set => field = value > MaxPageSize ? MaxPageSize : value < 1 ? 10 : value;
    } = 10;
}
