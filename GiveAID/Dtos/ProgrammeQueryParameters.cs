namespace GiveAID.Dtos;

public class ProgrammeQueryParameters
{
    // Search
    public string? SearchTerm { get; set; }

    // Filter
    public Guid? NgoId { get; set; }
    public Guid? CauseId { get; set; }

    // Pagination
    private const int MaxPageSize = 50;
    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get;
        set => field = value > MaxPageSize ? MaxPageSize : value;
    }
}
