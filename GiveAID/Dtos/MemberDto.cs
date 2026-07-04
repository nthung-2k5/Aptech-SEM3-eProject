namespace GiveAID.Dtos;

public record MemberSaveDto(string FullName, string Email, string Password, DateOnly Dob, string Address, string PhoneNumber, string Occupation);

public record MemberSummaryDto(Guid Id, string FullName, string Email, DateOnly Dob, string PhoneNumber);

public record MemberDto(Guid Id, string FullName, string Email, string Password, DateOnly Dob, string Address, string PhoneNumber, string Occupation): MemberSummaryDto(Id, FullName, Email, Dob, PhoneNumber);
