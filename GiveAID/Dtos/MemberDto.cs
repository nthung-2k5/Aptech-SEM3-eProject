using GiveAID.Models;

namespace GiveAID.Dtos;

public record MemberSaveDto(string FullName, string Email, string? Password, DateOnly DateOfBirth, string Address, string PhoneNumber, string Occupation);

public record MemberSummaryDto(Guid Id, string FullName, string Email, DateOnly DateOfBirth, string PhoneNumber);

public record MemberDto(Guid Id, string FullName, string Email, DateOnly DateOfBirth, string Address, string PhoneNumber, string Occupation): MemberSummaryDto(Id, FullName, Email, DateOfBirth, PhoneNumber);

public static class MemberMapper
{
    extension(User member)
    {
        public MemberSummaryDto ToSummaryDto() => new(member.UserId, member.FullName, member.Email, member.DateOfBirth, member.PhoneNumber);
        public MemberDto ToDto() => new(member.UserId, member.FullName, member.Email, member.DateOfBirth, member.Address, member.PhoneNumber, member.Occupation);
    }

    public static User ToEntity(this MemberSaveDto dto, string passwordHash) => new()
    {
        FullName = dto.FullName,
        Email = dto.Email,
        PasswordHash = passwordHash,
        DateOfBirth = dto.DateOfBirth,
        Address = dto.Address,
        PhoneNumber = dto.PhoneNumber,
        Occupation = dto.Occupation,
        Role = UserRole.Member
    };
}
