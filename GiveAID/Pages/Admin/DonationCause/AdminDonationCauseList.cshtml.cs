using GiveAID.Dtos;
using GiveAID.Exceptions;
using GiveAID.Services.Abstractions;
using Hydro;

namespace GiveAID.Pages.Admin.DonationCause;

public class AdminDonationCauseList(IDonationCauseService causeService) : HydroComponent
{
    public DonationCauseDto[] Causes { get; set; } = [];

    // State for create
    public bool IsCreating { get; set; }
    public string NewCauseName { get; set; } = string.Empty;

    // State for edit
    public Guid? EditingId { get; set; }
    public string EditingName { get; set; } = string.Empty;

    public override async Task MountAsync()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        Causes = await causeService.GetAllDonationCausesAsync();
    }

    public void StartCreate()
    {
        IsCreating = true;
        NewCauseName = string.Empty;
        EditingId = null;
        ModelState.Clear();
    }

    public void CancelCreate()
    {
        IsCreating = false;
        NewCauseName = string.Empty;
        ModelState.Clear();
    }

    public async Task SaveNew()
    {
        if (string.IsNullOrWhiteSpace(NewCauseName))
        {
            return;
        }

        try
        {
            await causeService.CreateDonationCauseAsync(new DonationCauseSaveDto(NewCauseName.Trim()));
            IsCreating = false;
            NewCauseName = string.Empty;
            ModelState.Clear();
            await LoadDataAsync();
            Client.ExecuteJs("Swal.fire('Success', 'Donation cause added successfully', 'success');");
        }
        catch (DuplicateException ex)
        {
            if (ex.FieldName == nameof(DonationCauseSaveDto.Name))
            {
                ModelState.AddModelError(nameof(NewCauseName), "The cause name already exists.");
            }
        }
    }

    public void StartEdit(Guid id, string currentName)
    {
        EditingId = id;
        EditingName = currentName;
        IsCreating = false;
        ModelState.Clear();
    }

    public void CancelEdit()
    {
        EditingId = null;
        EditingName = string.Empty;
        ModelState.Clear();
    }

    public async Task SaveEdit()
    {
        if (EditingId == null || string.IsNullOrWhiteSpace(EditingName))
        {
            return;
        }

        try
        {
            await causeService.UpdateDonationCauseAsync(EditingId.Value, new DonationCauseSaveDto(EditingName.Trim()));
            EditingId = null;
            EditingName = string.Empty;
            ModelState.Clear();
            await LoadDataAsync();
            Client.ExecuteJs("Swal.fire('Success', 'Donation cause updated successfully', 'success');");
        }
        catch (DuplicateException ex)
        {
            if (ex.FieldName == nameof(DonationCauseSaveDto.Name))
            {
                ModelState.AddModelError(nameof(EditingName), "The cause name already exists.");
            }
        }
    }

    public async Task Delete(Guid id)
    {
        await causeService.DeleteDonationCauseAsync(id);
        await LoadDataAsync();
        Client.ExecuteJs("Swal.fire('Deleted!', 'The record has been deleted.', 'success');");
    }
}
