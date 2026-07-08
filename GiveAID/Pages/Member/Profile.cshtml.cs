using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using GiveAID.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveAID.Pages.Member;

[Authorize(Roles = "Member")]
public class ProfileModel : PageModel
{
    private readonly AppDbContext _context;

    public ProfileModel(AppDbContext context) => _context = context;

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression(
            @"^(?:\+84|0)(?:3[2-9]|5[2|5|6|8|9]|7[0|6-9]|8[1-9]|9[0-4|6-9])[0-9]{7}$",
            ErrorMessage = "The phone number is not valid.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [MaxLength(255)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Occupation { get; set; } = string.Empty;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        string? userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdStr, out var userId)) { return RedirectToPage("/Login/Index"); }

        var user = await _context.Users.FindAsync(userId);

        if (user == null) { return RedirectToPage("/Login/Index"); }

        Input = new InputModel
        {
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            DateOfBirth = user.DateOfBirth,
            Address = user.Address,
            Occupation = user.Occupation
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) { return Page(); }

        string? userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdStr, out var userId)) { return RedirectToPage("/Login/Index"); }

        var user = await _context.Users.FindAsync(userId);

        if (user == null) { return RedirectToPage("/Login/Index"); }

        user.FullName = Input.FullName;
        user.PhoneNumber = Input.PhoneNumber;
        user.DateOfBirth = Input.DateOfBirth;
        user.Address = Input.Address;
        user.Occupation = Input.Occupation;

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Your profile has been updated successfully.";

        return RedirectToPage();
    }
}
