using GiveAID.Data;
using GiveAID.Dtos;
using GiveAID.Models;
using GiveAID.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Services;

public class NgoService(AppDbContext dbContext) : INgoService
{
        public async Task<NgoSummaryDto[]> GetAllNgosAsync(CancellationToken ct = default)
        {
                return await dbContext.Ngos
                    .AsNoTracking()
                    .Where(n => !n.IsDeleted)
                    .OrderByDescending(n => n.CreatedAt)
                    .Select(n => new NgoSummaryDto(n.NgoId, n.Name, n.Description))
                    .ToArrayAsync(ct);
        }

        public async Task<NgoDto?> GetNgoByIdAsync(Guid id, CancellationToken ct = default)
        {
                return await dbContext.Ngos
                    .AsNoTracking()
                    .Where(n => n.NgoId == id && !n.IsDeleted)
                    .Select(n => new NgoDto(
                        n.NgoId,
                        n.Name,
                        n.Description,
                        n.Address,
                        n.PhoneNumber,
                        n.Website ?? string.Empty))
                    .FirstOrDefaultAsync(ct);
        }

        public async Task<NgoDto?> CreateNgoAsync(NgoSaveDto ngo, CancellationToken ct = default)
        {
                // Reject if an active NGO with the same name already exists
                bool exists = await dbContext.Ngos.AnyAsync(n => n.Name == ngo.Name && !n.IsDeleted, ct);
                if (exists) return null;

                var entity = new Ngo
                {
                        Name = ngo.Name,
                        Description = ngo.Description,
                        Address = ngo.Address,
                        PhoneNumber = ngo.PhoneNumber,
                        Website = ngo.Website,
                };

                dbContext.Ngos.Add(entity);
                await dbContext.SaveChangesAsync(ct);

                return new NgoDto(
                    entity.NgoId,
                    entity.Name,
                    entity.Description,
                    entity.Address,
                    entity.PhoneNumber,
                    entity.Website ?? string.Empty);
        }

        public async Task<bool> UpdateNgoAsync(Guid id, NgoSaveDto ngo, CancellationToken ct = default)
        {
                // Reject if another active NGO already uses the same name
                bool nameConflict = await dbContext.Ngos
                    .AnyAsync(n => n.Name == ngo.Name && n.NgoId != id && !n.IsDeleted, ct);
                if (nameConflict) return false;

                var entity = await dbContext.Ngos
                    .FirstOrDefaultAsync(n => n.NgoId == id && !n.IsDeleted, ct);

                if (entity == null) return false;

                entity.Name = ngo.Name;
                entity.Description = ngo.Description;
                entity.Address = ngo.Address;
                entity.PhoneNumber = ngo.PhoneNumber;
                entity.Website = ngo.Website;

                await dbContext.SaveChangesAsync(ct);
                return true;
        }

        public async Task<bool> DeleteNgoAsync(Guid id, CancellationToken ct = default)
        {
                return await dbContext.Ngos
                    .Where(n => n.NgoId == id && !n.IsDeleted)
                    .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsDeleted, true), ct) > 0;
        }
}
