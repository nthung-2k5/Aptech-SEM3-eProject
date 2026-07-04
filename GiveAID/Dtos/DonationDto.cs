using OneOf;

namespace GiveAID.Dtos;

public record DonateForNgoTarget(Guid NgoId, Guid CauseId);

public record DonateForProgrammeTarget(Guid ProgrammeId);

[GenerateOneOf]
public partial class DonationTarget : OneOfBase<DonateForNgoTarget, DonateForProgrammeTarget>;

public record DonationSaveDto(Guid UserId, DonationTarget Target, decimal Amount);

public record UserDonationDto(DonationTarget Target, decimal Amount, DateTimeOffset DonationDate);

public record DonationDto(Guid Id, Guid DonorId, string DonorName, DonationTarget Target, decimal Amount, DateTimeOffset DonationDate);
