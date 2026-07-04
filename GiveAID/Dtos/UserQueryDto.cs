namespace GiveAID.Dtos;

public record UserQueryCreateDto(Guid UserId, string Subject, string MessageText);

public record UserQueryDto(Guid Id, Guid UserId, string UserName, string Subject, string MessageText, string? ReplyText, DateTimeOffset CreatedAt);
