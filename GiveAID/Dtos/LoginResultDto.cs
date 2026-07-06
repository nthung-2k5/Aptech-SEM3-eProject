using GiveAID.Models;

namespace GiveAID.Dtos;

public record LoginResultDto(Guid UserId, UserRole Role, string Token);
