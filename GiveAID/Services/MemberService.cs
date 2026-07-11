using GiveAID.Data;
using GiveAID.Dtos;
using GiveAID.Exceptions;
using GiveAID.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Services;

public class MemberService(AppDbContext dbContext, IPasswordService passwordService) : IMemberService
{
    public async Task<PagedResult<MemberDto>> GetMembersPagedAsync(MemberQueryParameters query, CancellationToken ct = default)
    {
        var q = dbContext.ActiveMembers.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            q = q.Where(m => m.FullName.Contains(query.SearchTerm) || m.Email.Contains(query.SearchTerm) || m.PhoneNumber.Contains(query.SearchTerm));
        }

        int totalCount = await q.CountAsync(ct);
        var items = await q.OrderByDescending(m => m.CreatedAt)
                .Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize)
                .ProjectToDto().ToArrayAsync(ct);

        return new PagedResult<MemberDto>(items, totalCount, query.PageNumber, query.PageSize);
    }

    public async Task<MemberSummaryDto[]> GetAllMembersAsync(string? searchTerm, int page = 1, int pageSize = 10,
                                                             CancellationToken ct = default)
    {
        var query = dbContext.ActiveMembers.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(m => m.FullName.Contains(searchTerm) || m.Email.Contains(searchTerm) || m.PhoneNumber.Contains(searchTerm));
        }

        var items = await query.OrderByDescending(m => m.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize)
                .ProjectToSummaryDto().ToArrayAsync(ct);

        return items;
    }

    public async Task<MemberDto[]> GetAllMemberDtosAsync(string? searchTerm, CancellationToken ct = default)
    {
        var query = dbContext.ActiveMembers.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(m => m.FullName.Contains(searchTerm) || m.Email.Contains(searchTerm));
        }

        return await query.OrderByDescending(m => m.CreatedAt).ProjectToDto().ToArrayAsync(ct);
    }

    public async Task<MemberDto?> GetMemberByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await dbContext.ActiveMembers.AsNoTracking().Where(m => m.UserId == id).ProjectToDto()
                .FirstOrDefaultAsync(ct);
    }

    public async Task<MemberDto> CreateMemberAsync(MemberCreateDto dto, CancellationToken ct = default)
    {
        if (await dbContext.Users.AnyAsync(u => u.Email == dto.Email, ct))
        {
            throw new DuplicateException(nameof(dto.Email));
        }

        if (await dbContext.Users.AnyAsync(u => u.PhoneNumber == dto.PhoneNumber, ct))
        {
            throw new DuplicateException(nameof(dto.PhoneNumber));
        }

        var entity = dto.ToEntity(passwordService.HashPassword(dto.Password));

        await dbContext.Users.AddAsync(entity, ct);
        await dbContext.SaveChangesAsync(ct);

        return entity.ToDto();
    }

    public async Task UpdateMemberAsync(Guid id, MemberUpdateDto dto, CancellationToken ct = default)
    {
        var user = await dbContext.Users.FindAsync([id], ct);

        if (user == null || user.IsDeleted) { throw new NotFoundException(); }

        if (await dbContext.Users.AnyAsync(u => u.Email == dto.Email && u.UserId != id, ct))
        {
            throw new DuplicateException(nameof(dto.Email));
        }

        if (await dbContext.Users.AnyAsync(u => u.PhoneNumber == dto.PhoneNumber && u.UserId != id, ct))
        {
            throw new DuplicateException(nameof(dto.PhoneNumber));
        }

        user.FullName = dto.FullName;
        user.Email = dto.Email;

        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            user.PasswordHash = passwordService.HashPassword(dto.Password);
        }

        user.DateOfBirth = dto.DateOfBirth;
        user.Address = dto.Address;
        user.PhoneNumber = dto.PhoneNumber;
        user.Occupation = dto.Occupation;

        await dbContext.SaveChangesAsync(ct);
    }

    public async Task DeleteMemberAsync(Guid id, CancellationToken ct = default)
    {
        await dbContext.ActiveMembers.Where(u => u.UserId == id)
                .ExecuteUpdateAsync(u => u.SetProperty(m => m.IsDeleted, true), ct);
    }
}
