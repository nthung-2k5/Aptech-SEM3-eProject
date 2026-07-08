using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Hydro;
using Microsoft.AspNetCore.Mvc;

namespace GiveAID.Pages.Admin.Programme;

public class AdminProgrammeEditor(
    IProgrammeService programmeService,
    INgoService ngoService,
    IDonationCauseService causeService) : HydroComponent
{
    public Guid? Id { get; set; }

    public class FormModel
    {
        public string Name { get; set; } = "";
        public Guid NgoId { get; set; }
        public Guid CauseId { get; set; }
        public string Description { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public string? Location { get; set; }
        public DateTimeOffset StartTime { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? EndTime { get; set; }
        public decimal? MaxDonation { get; set; }
    }

    public FormModel Form { get; set; } = new();
    public NgoSummaryDto[] AvailableNgos { get; set; } = [];
    public DonationCauseDto[] AvailableCauses { get; set; } = [];

    public override async Task MountAsync()
    {
        AvailableNgos = await ngoService.GetAllNgosAsync();
        AvailableCauses = await causeService.GetAllDonationCausesAsync();

        if (Id.HasValue && Id.Value != Guid.Empty)
        {
            var p = await programmeService.GetProgrammeSaveDtoByIdAsync(Id.Value);
            if (p != null)
            {
                Form = new FormModel
                {
                    Name = p.Name,
                    NgoId = p.NgoId,
                    CauseId = p.CauseId,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    Location = p.Location,
                    StartTime = p.StartTime,
                    EndTime = p.EndTime,
                    MaxDonation = p.MaxDonation
                };
            }
        }
    }

    public async Task Save()
    {
        var saveDto = new ProgrammeSaveDto(
            Form.NgoId,
            Form.CauseId,
            Form.Name,
            Form.ImageUrl,
            Form.Description,
            Form.StartTime,
            Form.EndTime,
            Form.MaxDonation,
            Form.Location
        );

        if (Id.HasValue && Id.Value != Guid.Empty) { await programmeService.UpdateProgrammeAsync(Id.Value, saveDto); }
        else { await programmeService.CreateProgrammeAsync(saveDto); }

        Redirect(Url.Page("/Admin/Programme"));
    }
}
