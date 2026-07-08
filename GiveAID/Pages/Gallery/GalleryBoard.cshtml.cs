using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Hydro;

namespace GiveAID.Pages.Gallery;

public class GalleryBoard(IGalleryImageService galleryService, IProgrammeService programmeService, IWebHostEnvironment env) : HydroComponent
{
    public GalleryImageDto[] Images { get; set; } = [];
    public ProgrammeDto[] Programmes { get; set; } = [];
    public string Filter { get; set; } = "All";
    
    public bool IsModalOpen { get; set; }
    public Guid? EditingId { get; set; }
    public string? PreviewImageUrl { get; set; }

    public class FormModel
    {
        public string Caption { get; set; } = "";
        public Guid? AssociatedProgrammeId { get; set; }
        public IFormFile? ImageFile { get; set; }
    }

    public FormModel Form { get; set; } = new();

    public override async Task MountAsync()
    {
        await LoadData();
    }

    private async Task LoadData()
    {
        Images = await galleryService.GetAllImagesAsync();
        Programmes = await programmeService.GetAllProgrammesAsync(null);
    }

    public void SetFilter(string filter)
    {
        Filter = filter;
    }

    public void OpenCreateModal()
    {
        EditingId = null;
        Form = new FormModel();
        PreviewImageUrl = null;
        IsModalOpen = true;
    }

    public async Task OpenEditModal(Guid id)
    {
        EditingId = id;
        var image = await galleryService.GetImageByIdAsync(id);
        if (image != null)
        {
            Form = new FormModel 
            { 
                Caption = image.Caption, 
                AssociatedProgrammeId = image.AssociatedProgramme?.Id 
            };
            PreviewImageUrl = image.ImageUri.ToString();
            IsModalOpen = true;
        }
    }

    public void CloseModal()
    {
        IsModalOpen = false;
    }

    public async Task Save()
    {
        var imageUri = new Uri("http://localhost/images/gallery/default.jpg");

        if (EditingId.HasValue)
        {
            var existing = await galleryService.GetImageByIdAsync(EditingId.Value);
            if (existing != null)
            {
                imageUri = existing.ImageUri;
            }
        }

        if (Form.ImageFile is { Length: > 0 })
        {
            string uploadsFolder = Path.Combine(env.WebRootPath, "images", "gallery");
            Directory.CreateDirectory(uploadsFolder);
            string uniqueFileName = Guid.NewGuid() + "_" + Form.ImageFile.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await Form.ImageFile.CopyToAsync(stream);
            }
            
            imageUri = new Uri($"/images/gallery/{uniqueFileName}", UriKind.RelativeOrAbsolute);
        }

        var saveDto = new GalleryImageSaveDto(
            imageUri,
            Form.Caption,
            Form.AssociatedProgrammeId);

        if (!EditingId.HasValue || EditingId.Value == Guid.Empty)
        {
            await galleryService.UploadImageAsync(saveDto);
        }
        else
        {
            await galleryService.UpdateImageAsync(EditingId.Value, saveDto);
        }

        IsModalOpen = false;
        await LoadData();
    }

    public async Task Delete(Guid id)
    {
        await galleryService.DeleteImageAsync(id);
        await LoadData();
    }
}
