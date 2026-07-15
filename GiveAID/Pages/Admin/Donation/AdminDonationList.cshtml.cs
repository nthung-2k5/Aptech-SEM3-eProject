using GiveAID.Dtos;
using GiveAID.Models;
using GiveAID.Services.Abstractions;
using Hydro;

namespace GiveAID.Pages.Admin.Donation;

public class AdminDonationList(
    IDonationService donationService, 
    IMemberService memberService,
    IProgrammeService programmeService,
    INgoService ngoService,
    IDonationCauseService causeService) : HydroComponent
{
    // Filter / Search state
    public string? DateFrom { get; set; }
    public string? DateTo { get; set; }
    public string StatusFilter { get; set; } = string.Empty; // "" | "Completed" | "Void"
    public string? ProgrammeId { get; set; }
    public string? NgoId { get; set; }
    public string? CauseId { get; set; }
    public int PageNumber { get; set; } = 1;
    private const int PageSize = 10;

    // Results
    public DonationDto[] Donations { get; set; } = [];
    public int TotalCount { get; set; }
    public int TotalPages => TotalCount == 0 ? 1 : (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
    public decimal TotalAmount => Donations.Sum(d => d.Amount);

    public ProgrammeDto[] AvailableProgrammes { get; set; } = [];
    public NgoSummaryDto[] AvailableNgos { get; set; } = [];
    public DonationCauseDto[] AvailableCauses { get; set; } = [];

    public override async Task MountAsync() 
    { 
        AvailableProgrammes = (await programmeService.GetAllProgrammesPagedAsync(new ProgrammeQueryParameters { PageSize = 1000 })).Items;
        AvailableNgos = await ngoService.GetAllNgosAsync();
        AvailableCauses = await causeService.GetAllDonationCausesAsync();
        await LoadDataAsync(); 
    }

    public async Task Search()
    {
        PageNumber = 1;
        await LoadDataAsync();
    }

    public async Task GoToPage(int page)
    {
        PageNumber = page;
        await LoadDataAsync();
    }

    public async Task Void(Guid id)
    {
        await donationService.VoidDonationAsync(id);
        await LoadDataAsync();
        Client.ExecuteJs("Swal.fire('Voided!', 'The donation has been voided.', 'success');");
    }

    public bool OpenModal { get; set; }
    public MemberDto? SelectedMember { get; set; }

    public async Task ShowMemberInfo(Guid memberId)
    {
        var member = await memberService.GetMemberByIdAsync(memberId);
        OpenModal = true;
        if (member != null) { SelectedMember = member; }
    }

    public void CloseModal() { OpenModal = false; }

    private async Task LoadDataAsync()
    {
        var query = new DonationQueryParameters
        {
            DateFrom = !string.IsNullOrEmpty(DateFrom) ? DateOnly.Parse(DateFrom) : null,
            DateTo   = !string.IsNullOrEmpty(DateTo)   ? DateOnly.Parse(DateTo)   : null,
            Status = StatusFilter switch
            {
                "Completed" => DonationStatus.Completed,
                "Void" => DonationStatus.Void,
                _ => null
            },
            ProgrammeId = !string.IsNullOrEmpty(ProgrammeId) ? Guid.Parse(ProgrammeId) : null,
            NgoId = !string.IsNullOrEmpty(NgoId) ? Guid.Parse(NgoId) : null,
            CauseId = !string.IsNullOrEmpty(CauseId) ? Guid.Parse(CauseId) : null,
            PageNumber = PageNumber,
            PageSize = PageSize
        };

        var result = await donationService.GetDonationsPagedAsync(query);
        Donations = result.Items;
        TotalCount = result.TotalCount;
    }
}
