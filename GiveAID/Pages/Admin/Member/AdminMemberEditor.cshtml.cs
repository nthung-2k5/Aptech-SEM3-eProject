using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Hydro;
using Microsoft.AspNetCore.Mvc;

namespace GiveAID.Pages.Admin.Member;

public class AdminMemberEditor(IMemberService memberService) : HydroComponent
{
    public Guid? Id { get; set; }

    // We bind to a SaveDto class equivalent for the form
    public class FormModel
    {
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Password { get; set; } = "";
        public string Address { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Occupation { get; set; } = "";
        public DateOnly DateOfBirth { get; set; } = new(1990, 1, 1);
    }

    public FormModel Form { get; set; } = new();

    public override async Task MountAsync()
    {
        if (Id.HasValue && Id.Value != Guid.Empty)
        {
            var m = await memberService.GetMemberByIdAsync(Id.Value);

            if (m != null)
            {
                Form = new FormModel
                {
                    FullName = m.FullName, Email = m.Email, Address = m.Address,
                    PhoneNumber = m.PhoneNumber, Occupation = m.Occupation, DateOfBirth = m.DateOfBirth
                };
            }
        }
    }

    public async Task Save()
    {
        if (Id.HasValue && Id.Value != Guid.Empty)
        {
            var updateDto = new MemberUpdateDto(
                Form.FullName,
                Form.Email,
                Form.Password,
                Form.DateOfBirth,
                Form.Address,
                Form.PhoneNumber,
                Form.Occupation);

            await memberService.UpdateMemberAsync(Id.Value, updateDto);
        }
        else
        {
            if (string.IsNullOrWhiteSpace(Form.Password))
            {
                throw new InvalidOperationException("Password is required for new members.");
            }

            var createDto = new MemberCreateDto(
                Form.FullName,
                Form.Email,
                Form.Password,
                Form.DateOfBirth,
                Form.Address,
                Form.PhoneNumber,
                Form.Occupation);

            await memberService.CreateMemberAsync(createDto);
        }

        Redirect(Url.Page("/Admin/Member"));
    }
}
