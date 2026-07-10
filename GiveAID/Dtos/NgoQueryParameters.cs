namespace GiveAID.Dtos;

public class NgoQueryParameters
{
    private const int MaxPageSize = 50;

    public string? SearchTerm { get; set; }
    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get;
        set => field = value > MaxPageSize ? MaxPageSize : value < 1 ? 10 : value;
    } = 10;
}
