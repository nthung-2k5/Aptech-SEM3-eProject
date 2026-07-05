using System.ComponentModel.DataAnnotations;
using Hydro;

namespace GiveAID.Pages.Register;

public class RegisterForm : HydroComponent
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Phone number is required")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Enter 10-digit phone number")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Date of Birth is required")]
    public string DOB { get; set; }

    [MinLength(1, ErrorMessage = "Occupation is required")]
    public string Occupation { get; set; }
    public string Address { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Min 6 characters")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm password is required")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }

    [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the terms")]
    public bool Agree { get; set; }

    public bool IsSuccess { get; set; }

    public void Submit()
    {
        if (!Validate())
        {
            return;
        }
        
        if (ModelState.IsValid) { IsSuccess = true; }
    }
}
