using System.ComponentModel.DataAnnotations;

namespace GiveAID.Models;

public class GalleryImage
{
    [Key]
    public Guid ImageId { get; set; } = Guid.CreateVersion7();

    [MaxLength(2048)]
    public string ImageUrl { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Caption { get; set; }

    public Guid? ProgrammeId { get; set; }
    public WelfareProgramme? Programme { get; set; }
}
