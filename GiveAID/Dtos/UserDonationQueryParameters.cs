namespace GiveAID.Dtos;

public class UserDonationQueryParameters
{
    private const int MaxPageSize = 50;

    public string TimePeriod { get; set; } = "all";
    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }
    public bool TargetNgo { get; set; }
    public bool TargetProgramme { get; set; }
    
    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get;
        set => field = value > MaxPageSize ? MaxPageSize : value < 1 ? 10 : value;
    } = 10;
}
