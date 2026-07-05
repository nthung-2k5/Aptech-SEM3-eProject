using GiveAID.Data;
using GiveAID.Models;
using GiveAID.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Services;

public class MemberService(AppDbContext dbContext) : IMemberService
{
    private readonly PasswordHasher<Member> _passwordHasher = new();

    public async Task<PagedResult<Member>> GetMembersAsync(
        string? searchTerm,
        int page = 1,
        int pageSize = 10,
        CancellationToken ct = default)
    {
        var query = dbContext.Members.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(m =>
                m.FullName.Contains(searchTerm) ||
                m.Email.Contains(searchTerm));
        }

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(m => m.RegisteredAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<Member>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<Member?> GetMemberByIdAsync(Guid memberId, CancellationToken ct = default)
    {
        return await dbContext.Members
            .FirstOrDefaultAsync(m => m.MemberId == memberId, ct);
    }

    public async Task<Member> CreateMemberAsync(Member member, CancellationToken ct = default)
    {
        member.IsActive = true;
        member.RegisteredAt = DateTime.UtcNow;

        dbContext.Members.Add(member);
        await dbContext.SaveChangesAsync(ct);
        return member;
    }

    public async Task<Member> UpdateMemberAsync(Member member, CancellationToken ct = default)
    {
        member.UpdatedAt = DateTime.UtcNow;
        dbContext.Members.Update(member);
        await dbContext.SaveChangesAsync(ct);
        return member;
    }

    public async Task<bool> DeactivateMemberAsync(Guid memberId, CancellationToken ct = default)
    {
        var member = await dbContext.Members
            .FirstOrDefaultAsync(m => m.MemberId == memberId, ct);

        if (member == null)
        {
            return false;
        }

        member.IsActive = false;
        member.UpdatedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(ct);
        return true;
    }

    public async Task<RegisterResult> RegisterMemberAsync(Member member, string plainPassword, CancellationToken ct = default)
    {
        var emailExists = await dbContext.Members
            .AnyAsync(m => m.Email == member.Email, ct);

        if (emailExists)
        {
            return RegisterResult.Fail("Email address is already registered.");
        }

        if (string.IsNullOrWhiteSpace(plainPassword) || plainPassword.Length < 6)
        {
            return RegisterResult.Fail("Password must be at least 6 characters.");
        }

        member.PasswordHash = _passwordHasher.HashPassword(member, plainPassword);
        member.IsActive = true;
        member.RegisteredAt = DateTime.UtcNow;

        dbContext.Members.Add(member);
        await dbContext.SaveChangesAsync(ct);

        return RegisterResult.Ok(member);
    }

    public async Task<UpdateProfileResult> UpdateMemberProfileAsync(
        Guid memberId,
        string? fullName,
        DateTime? dateOfBirth,
        string? address,
        string? occupation,
        CancellationToken ct = default)
    {
        var member = await dbContext.Members
            .FirstOrDefaultAsync(m => m.MemberId == memberId, ct);

        if (member == null)
        {
            return UpdateProfileResult.Fail("Member not found.");
        }

        // Validate: nếu có nhập full name thì không được rỗng
        if (fullName != null && string.IsNullOrWhiteSpace(fullName))
        {
            return UpdateProfileResult.Fail("Full name cannot be empty.");
        }

        // Validate: ngày sinh không được ở tương lai
        if (dateOfBirth.HasValue && dateOfBirth.Value > DateTime.UtcNow)
        {
            return UpdateProfileResult.Fail("Date of birth cannot be in the future.");
        }

        // Chỉ cập nhật field nào có giá trị (optional fields)
        if (fullName != null) member.FullName = fullName;
        if (dateOfBirth.HasValue) member.DateOfBirth = dateOfBirth;
        if (address != null) member.Address = address;
        if (occupation != null) member.Occupation = occupation;

        member.UpdatedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(ct);

        return UpdateProfileResult.Ok(member);
    }
}