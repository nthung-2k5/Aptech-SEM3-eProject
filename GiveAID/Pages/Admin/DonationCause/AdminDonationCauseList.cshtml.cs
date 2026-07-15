using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Hydro;

namespace GiveAID.Pages.Admin.DonationCause;

public class AdminDonationCauseList(IDonationCauseService donationCauseService) : HydroComponent
{
    // Filter / Search state
    public string SearchTerm { get; set; } = string.Empty;

    // Results
    public DonationCauseDto[] Causes { get; set; } = [];
    public int TotalCount { get; set; }

    public override async Task MountAsync() { await LoadDataAsync(); }

    public async Task Search()
    {
        await LoadDataAsync();
    }

    public async Task Delete(Guid id)
    {
        await donationCauseService.DeleteDonationCauseAsync(id);
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        var allCauses = await donationCauseService.GetAllDonationCausesAsync();
        
        if (!string.IsNullOrWhiteSpace(SearchTerm))
        {
            Causes = allCauses.Where(c => c.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)).ToArray();
        }
        else
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
