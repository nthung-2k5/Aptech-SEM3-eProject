using FluentValidation;
using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Hydro;
using Hydro.Utils;

namespace GiveAID.Pages.Gallery;

public class GalleryBoard(
    IGalleryImageService galleryService,
    IProgrammeService programmeService,
    IDonationCauseService causeService,
    IImageService imageService,
    IValidator<GalleryBoard> validator
) : HydroComponent
{
    public GalleryImageDto[] Images { get; set; } = [];
    public ProgrammeDto[] Programmes { get; set; } = [];
    public DonationCauseDto[] Causes { get; set; } = [];
    public string Filter { get; set; } = "All";

    public GalleryImageDto[] FilteredImages =>
            Filter == "All"
                    ? Images
                    : Images.Where(img =>
                                    img.AssociatedProgramme.HasValue &&
                                    Programmes.FirstOrDefault(p => p.Id == img.AssociatedProgramme.Value.Id)?.Cause ==
                                    Filter)
                            .ToArray();

    public bool IsModalOpen { get; set; }
    public Guid? EditingId { get; set; }
    public string? PreviewImageSource { get; set; }

    public class FormModel
    {
        public string Caption { get; set; } = "";
        public Guid? AssociatedProgrammeId { get; set; }
        public string? NewImageExtension { get; set; }
    }

    [Transient]
    public IFormFile? ImageFile { get; set; }

    public FormModel Form { get; set; } = new();

    public override async Task MountAsync() { await LoadData(); }

    private async Task LoadData()
    {
        Images = await galleryService.GetAllImagesAsync();
        Programmes = await programmeService.GetAllProgrammesAsync(null);
        Causes = await causeService.GetAllDonationCausesAsync();
    }

    public void SetFilter(string filter) { Filter = filter; }

    public override async Task BindAsync(PropertyPath property, object value)
    {
        if (property.Name == nameof(ImageFile))
        {
            var image = (IFormFile)value;
            PreviewImageSource = await image.ToDataUrlAsync();
            Form.NewImageExtension = Path.GetExtension(image.FileName);
        }
    }

    public void OpenCreateModal()
    {
        EditingId = null;
        Form = new FormModel();
        ImageFile = null;
        PreviewImageSource = null;
        IsModalOpen = true;
    }

    public async Task OpenEditModal(Guid id)
    {
        EditingId = id;
        var image = await galleryService.GetImageByIdAsync(id);
        ImageFile = null;

        if (image != null)
        {
            Form = new FormModel
            {
                Caption = image.Caption,
                AssociatedProgrammeId = image.AssociatedProgramme?.Id
            };
            PreviewImageSource = image.ImageUri;
            IsModalOpen = true;
        }
    }

    public void CloseModal() { IsModalOpen = false; }

    public async Task Save()
    {
        if (!this.Validate(validator)) { return; }

        string imageUri = "";

        if (EditingId.HasValue)
        {
            var existing = await galleryService.GetImageByIdAsync(EditingId.Value);

            if (existing != null) { imageUri = existing.ImageUri; }
        }

        if (Form.NewImageExtension != null)
        {
            string base64Data = PreviewImageSource!.Split(',')[1];
            byte[] bytes = Convert.FromBase64String(base64Data);

            if (EditingId.HasValue && EditingId.Value != Guid.Empty && PreviewImageSource.StartsWith("data:"))
            {
                imageUri = await imageService.UpdateImageAsync(imageUri, bytes);
            }
            else { imageUri = await imageService.UploadImageAsync("gallery", Form.NewImageExtension, bytes); }
        }

        var saveDto = new GalleryImageSaveDto(imageUri, Form.Caption, Form.AssociatedProgrammeId);

        if (!EditingId.HasValue || EditingId.Value == Guid.Empty) { await galleryService.UploadImageAsync(saveDto); }
        else { await galleryService.UpdateImageAsync(EditingId.Value, saveDto); }

        IsModalOpen = false;
        await LoadData();
        Client.ExecuteJs("Swal.fire('Success', 'Gallery image saved successfully', 'success');");
    }

    public async Task Delete(Guid id)
    {
        await galleryService.DeleteImageAsync(id);
        await LoadData();
        Client.ExecuteJs("Swal.fire('Deleted!', 'The record has been deleted.', 'success');");
    }

    public class Validator : AbstractValidator<GalleryBoard>
    {
        public Validator()
        {
            RuleFor(x => x.Form.Caption)
                .NotEmpty().WithMessage("Caption is required")
                .MaximumLength(255).WithMessage("Caption cannot exceed 255 characters");

            RuleFor(x => x.PreviewImageSource)
                .NotEmpty().WithMessage("Image file is required");
        }
    }
}
