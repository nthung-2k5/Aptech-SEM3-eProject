using GiveAID.Models;

namespace GiveAID.Dtos;

public class DonationQueryParameters
{
    private const int MaxPageSize = 50;

    public DateOnly? DateFrom { get; set; }
    public DateOnly? DateTo { get; set; }
    public DonationStatus? Status { get; set; }
    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get;
        set => field = value > MaxPageSize ? MaxPageSize : value < 1 ? 10 : value;
    } = 10;
}
