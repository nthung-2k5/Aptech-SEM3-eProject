using GiveAID.Data;
using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Services;

public class MemberService(AppDbContext dbContext, IPasswordService passwordService) : IMemberService
{
    public async Task<MemberSummaryDto[]> GetAllMembersAsync(string? searchTerm, int page = 1, int pageSize = 10,
                                                             CancellationToken ct = default)
    {
        var query = dbContext.Members.AsNoTracking().Where(m => !m.IsDeleted).AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(m => m.FullName.Contains(searchTerm) || m.Email.Contains(searchTerm));
        }

        var items = await query.OrderByDescending(m => m.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize)
                .Select(u => new MemberSummaryDto(u.UserId, u.FullName, u.Email, u.DateOfBirth, u.PhoneNumber))
                .ToArrayAsync(ct);

        return items;
    }

    public async Task<MemberDto?> GetMemberByIdAsync(Guid id, CancellationToken ct = default)
    {
        var user = await dbContext.Members.AsNoTracking().FirstOrDefaultAsync(m => m.UserId == id && !m.IsDeleted, ct);

        return user != null
                ? new MemberDto(
                    user.UserId,
                    user.FullName,
                    user.Email,
                    user.DateOfBirth,
                    user.Address,
                    user.PhoneNumber,
                    user.Occupation)
                : null;
    }

    public async Task<bool> CreateMemberAsync(MemberSaveDto member, CancellationToken ct = default)
    {
        if (member.Password == null)
        {
            throw new ArgumentException("Password cannot be null when creating a new member.");
        }
        
        bool exists = await dbContext.Users.AnyAsync(
            u => u.Email == member.Email || u.PhoneNumber == member.PhoneNumber,
            ct);

        if (exists) { return false; }

        var user = member.ToEntity(passwordService.HashPassword(member.Password));
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> UpdateMemberAsync(Guid id, MemberSaveDto member, CancellationToken ct = default)
    {
        var user = await dbContext.Members.FirstOrDefaultAsync(u => u.UserId == id && !u.IsDeleted, ct);

        if (user != null)
        {
            user.FullName = member.FullName;
            user.Email = member.Email;

            if (!string.IsNullOrWhiteSpace(member.Password))
            {
                user.PasswordHash = passwordService.HashPassword(member.Password);
            }

            user.DateOfBirth = member.DateOfBirth;
            user.Address = member.Address;
            user.PhoneNumber = member.PhoneNumber;
            user.Occupation = member.Occupation;

            await dbContext.SaveChangesAsync(ct);
            return true;
        }

        return false;
    }

    public async Task<bool> DeleteMemberAsync(Guid id, CancellationToken ct = default)
    {
        return await dbContext.Members.Where(u => u.UserId == id && !u.IsDeleted)
                .ExecuteUpdateAsync(u => u.SetProperty(m => m.IsDeleted, true), ct) > 0;
    }
}
