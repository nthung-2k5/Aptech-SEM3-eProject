using GiveAID.Models;
using OneOf;

namespace GiveAID.Dtos;

public record DonateForNgoTarget(Guid NgoId, Guid? CauseId);

public record DonateForProgrammeTarget(Guid ProgrammeId);

public record DonateForNgoTargetDto((Guid Id, string Name) Ngo, (Guid Id, string Name)? Cause);

public record DonateForProgrammeTargetDto(Guid ProgrammeId, string ProgrammeName);

[GenerateOneOf]
public partial class DonationTarget : OneOfBase<DonateForNgoTarget, DonateForProgrammeTarget>;

[GenerateOneOf]
public partial class DonationTargetDto : OneOfBase<DonateForNgoTargetDto, DonateForProgrammeTargetDto>;

public record DonationSaveDto(Guid UserId, DonationTarget Target, decimal Amount, Guid TransactionId);

public record UserDonationDto(DonationTargetDto Target, decimal Amount, DateTimeOffset DonationDate);

public record DonationDto(Guid Id, Guid DonorId, string DonorName, DonationTargetDto Target, decimal Amount, DateTimeOffset DonationDate, DonationStatus Status);

public static class DonationMapper
{
    public static DonationDto ToDto(this Donation donation) => new(
        donation.DonationId,
        donation.UserId,
        donation.User.FullName,
        GetTarget(donation),
        donation.Amount,
        donation.CreatedAt,
        donation.Status
    );

    private static DonationTargetDto GetTarget(Donation donation)
    {
        if (donation.Ngo != null)
        {
            return new DonateForNgoTargetDto((donation.Ngo.NgoId, donation.Ngo.Name), donation.Cause != null ? (donation.Cause.CauseId, donation.Cause.Name) : null);
        }

        return donation.Programme != null ? new DonateForProgrammeTargetDto(donation.Programme.ProgrammeId, donation.Programme.Name) : throw new InvalidOperationException("Donation must have either an NGO or a Programme target.");
    }
    
    public static IQueryable<UserDonationDto> ProjectToUserDto(this IQueryable<Donation> donations) =>
            donations.Select(d => new UserDonationDto(
                GetTarget(d),
                d.Amount,
                d.CreatedAt
            ));
    
    public static IQueryable<DonationDto> ProjectToDto(this IQueryable<Donation> donations) =>
        donations.Select(d => new DonationDto(
            d.DonationId,
            d.UserId,
            d.User.FullName,
            GetTarget(d),
            d.Amount,
            d.CreatedAt,
            d.Status
        ));
    
    public static Donation ToEntity(this DonationSaveDto dto) => new()
    {
        UserId = dto.UserId,
        NgoId = dto.Target.Match<Guid?>(
            ngo => ngo.NgoId,
            _ => null
        ),
        CauseId = dto.Target.Match(
            ngo => ngo.CauseId,
            _ => null
        ),
        ProgrammeId = dto.Target.Match<Guid?>(
            _ => null,
            prog => prog.ProgrammeId
        ),
        Amount = dto.Amount,
        TransactionId = dto.TransactionId,
        Status = DonationStatus.Completed
    };
}
