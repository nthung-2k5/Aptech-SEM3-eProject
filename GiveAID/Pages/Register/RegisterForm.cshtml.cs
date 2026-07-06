using System.ComponentModel.DataAnnotations;
using GiveAID.Dtos;
using GiveAID.Models;
using GiveAID.Services.Abstractions;
using Hydro;

namespace GiveAID.Pages.Register;

public class RegisterForm(IMemberService memberService) : HydroComponent
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

    [Required(ErrorMessage = "Occupation is required")]
    public string? Occupation { get; set; }
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

    public async Task Submit()
    {
        if (!Validate())
        {
            return;
        }
        
        if (ModelState.IsValid)
        {
            var user = new MemberSaveDto(
                Name,
                Email,
                Password,
                DateOnly.Parse(DOB),
                Address,
                Phone,
                Occupation ?? string.Empty);

            await memberService.CreateMemberAsync(user);

            // if (result.Success)
            // {
            //     IsSuccess = true; 
            // }
            // else
            // {
            //     // Optionally handle registration errors here (e.g. email already registered)
            // }
        }
    }
}
