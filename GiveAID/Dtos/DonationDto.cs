using GiveAID.Models;
using Newtonsoft.Json;
using OneOf;
using OneOf.Serialization;

namespace GiveAID.Dtos;

public class DonateForNgoTarget(Guid ngoId, Guid? causeId) : OneOfCase
{
    public Guid NgoId { get; } = ngoId;
    public Guid? CauseId { get; } = causeId;
}

public class DonateForProgrammeTarget(Guid programmeId) : OneOfCase
{
    public Guid ProgrammeId { get; } = programmeId;
}

public class DonateForNgoTargetDto((Guid, string) ngo, (Guid, string)? cause) : OneOfCase
{
    public (Guid Id, string Name) Ngo { get; } = ngo;
    public (Guid Id, string Name)? Cause { get; } = cause;
}

public class DonateForProgrammeTargetDto(Guid programmeId, string programmeName) : OneOfCase
{
    public Guid ProgrammeId { get; } = programmeId;
    public string ProgrammeName { get; } = programmeName;
}

[JsonConverter(typeof(OneOfJsonConverter<DonationTarget>))]
[GenerateOneOf]
public partial class DonationTarget : OneOfBase<DonateForNgoTarget, DonateForProgrammeTarget>
{
    public DonationTarget(DonateForNgoTarget ngoTarget) : base(ngoTarget) { }
    public DonationTarget(DonateForProgrammeTarget programmeTarget) : base(programmeTarget) { }
}

[JsonConverter(typeof(OneOfJsonConverter<DonationTargetDto>))]
[GenerateOneOf]
public partial class DonationTargetDto : OneOfBase<DonateForNgoTargetDto, DonateForProgrammeTargetDto>
{
    public DonationTargetDto(DonateForNgoTargetDto ngoTarget) : base(ngoTarget) { }
    public DonationTargetDto(DonateForProgrammeTargetDto programmeTarget) : base(programmeTarget) { }
}

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
    
    extension(IQueryable<Donation> donations)
    {
        public IQueryable<UserDonationDto> ProjectToUserDto() =>
                donations.Select(d => new UserDonationDto(
                    GetTarget(d),
                    d.Amount,
                    d.CreatedAt
                ));

        public IQueryable<DonationDto> ProjectToDto() =>
                donations.Select(d => new DonationDto(
                    d.DonationId,
                    d.UserId,
                    d.User.FullName,
                    GetTarget(d),
                    d.Amount,
                    d.CreatedAt,
                    d.Status
                ));
    }

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
