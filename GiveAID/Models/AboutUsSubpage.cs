using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Models;

[Index(nameof(Slug), IsUnique = true)]
[Index(nameof(Title), IsUnique = true)]
public class AboutUsSubpage
{
    [Key]
    public Guid SubpageId { get; set; } = Guid.CreateVersion7();
    
    [MaxLength(255)]
    public string Slug { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    public string HtmlContent => UserModifications.First().HtmlContent;

    public ICollection<UserModification> UserModifications { get; set; } = new List<UserModification>();
}
