using GiveAID.Models;
using GiveAID.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context, IPasswordService passwordService)
    {
        // 1. Seed Users
        if (!await context.Users.AnyAsync())
        {
            var users = new List<User>
            {
                new()
                {
                    UserId = Guid.CreateVersion7(),
                    FullName = "Admin User",
                    Email = "admin@giveaid.com",
                    PasswordHash = passwordService.HashPassword("Admin@123"),
                    DateOfBirth = new DateOnly(1980, 1, 1),
                    Address = "123 Admin St, City, Country",
                    PhoneNumber = "0987654321",
                    Occupation = "Administrator",
                    Role = UserRole.Admin,
                    IsDeleted = false
                },
                new()
                {
                    UserId = Guid.CreateVersion7(),
                    FullName = "John Doe",
                    Email = "john@example.com",
                    PasswordHash = passwordService.HashPassword("Member@123"),
                    DateOfBirth = new DateOnly(1990, 5, 15),
                    Address = "456 Member Ave, City, Country",
                    PhoneNumber = "0912345678",
                    Occupation = "Software Engineer",
                    Role = UserRole.Member,
                    IsDeleted = false
                },
                new()
                {
                    UserId = Guid.CreateVersion7(),
                    FullName = "Jane Smith",
                    Email = "jane@example.com",
                    PasswordHash = passwordService.HashPassword("Member@123"),
                    DateOfBirth = new DateOnly(1995, 8, 20),
                    Address = "789 User Blvd, City, Country",
                    PhoneNumber = "0934567890",
                    Occupation = "Teacher",
                    Role = UserRole.Member,
                    IsDeleted = false
                }
            };
            await context.Users.AddRangeAsync(users);
        }

        // 2. Seed Donation Causes
        if (!await context.DonationCauses.AnyAsync())
        {
            var causes = new List<DonationCause>
            {
                new() { CauseId = Guid.CreateVersion7(), Name = "Education" },
                new() { CauseId = Guid.CreateVersion7(), Name = "Healthcare" },
                new() { CauseId = Guid.CreateVersion7(), Name = "Environment" },
                new() { CauseId = Guid.CreateVersion7(), Name = "Disaster Relief" },
                new() { CauseId = Guid.CreateVersion7(), Name = "Animal Welfare" }
            };
            await context.DonationCauses.AddRangeAsync(causes);
        }
        
        await context.SaveChangesAsync();

        // 3. Seed NGOs
        if (!await context.Ngos.AnyAsync())
        {
            var ngos = new List<Ngo>
            {
                new()
                {
                    NgoId = Guid.CreateVersion7(),
                    Name = "Save The Children",
                    Description = "Working to improve the lives of children through better education, health care, and economic opportunities.",
                    Address = "100 Charity Lane, NGO City",
                    PhoneNumber = "0900000001",
                    Website = "https://savethechildren.org"
                },
                new()
                {
                    NgoId = Guid.CreateVersion7(),
                    Name = "Green Earth Foundation",
                    Description = "Dedicated to environmental conservation and fighting climate change.",
                    Address = "200 Green St, Eco Town",
                    PhoneNumber = "0900000002",
                    Website = "https://greenearth.org"
                },
                new()
                {
                    NgoId = Guid.CreateVersion7(),
                    Name = "Health First Aid",
                    Description = "Providing medical assistance and health education to underprivileged communities.",
                    Address = "300 Medical Ave, Health City",
                    PhoneNumber = "0900000003",
                    Website = "https://healthfirst.org"
                }
            };
            await context.Ngos.AddRangeAsync(ngos);
        }

        await context.SaveChangesAsync();

        // 4. Seed Corporate Partners
        if (!await context.CorporatePartners.AnyAsync())
        {
            var partners = new List<CorporatePartner>
            {
                new()
                {
                    PartnerId = Guid.CreateVersion7(),
                    Name = "Tech Corp",
                    Description = "A leading technology company supporting digital education.",
                    LogoUrl = "https://via.placeholder.com/150",
                    WebsiteLink = "https://techcorp.example.com"
                },
                new()
                {
                    PartnerId = Guid.CreateVersion7(),
                    Name = "Global Bank",
                    Description = "Financial institution committed to community development.",
                    LogoUrl = "https://via.placeholder.com/150",
                    WebsiteLink = "https://globalbank.example.com"
                }
            };
            await context.CorporatePartners.AddRangeAsync(partners);
        }

        await context.SaveChangesAsync();

        // 5. Seed Welfare Programmes
        if (!await context.WelfareProgrammes.AnyAsync())
        {
            var educationCause = await context.DonationCauses.FirstOrDefaultAsync(c => c.Name == "Education");
            var healthCause = await context.DonationCauses.FirstOrDefaultAsync(c => c.Name == "Healthcare");
            var environmentCause = await context.DonationCauses.FirstOrDefaultAsync(c => c.Name == "Environment");

            var saveTheChildren = await context.Ngos.FirstOrDefaultAsync(n => n.Name == "Save The Children");
            var greenEarth = await context.Ngos.FirstOrDefaultAsync(n => n.Name == "Green Earth Foundation");
            var healthFirst = await context.Ngos.FirstOrDefaultAsync(n => n.Name == "Health First Aid");

            if (educationCause != null && saveTheChildren != null)
            {
                await context.WelfareProgrammes.AddAsync(new WelfareProgramme
                {
                    ProgrammeId = Guid.CreateVersion7(),
                    NgoId = saveTheChildren.NgoId,
                    CauseId = educationCause.CauseId,
                    Name = "Rural School Building Project",
                    ImageUrl = "https://via.placeholder.com/800x400?text=School+Building",
                    Description = "Building schools in rural areas to provide access to education for thousands of children.",
                    StartTime = DateTimeOffset.UtcNow.AddDays(-10),
                    EndTime = DateTimeOffset.UtcNow.AddDays(100),
                    MaxDonation = 50000m,
                    Location = "Rural District, Countryside"
                });
            }

            if (environmentCause != null && greenEarth != null)
            {
                await context.WelfareProgrammes.AddAsync(new WelfareProgramme
                {
                    ProgrammeId = Guid.CreateVersion7(),
                    NgoId = greenEarth.NgoId,
                    CauseId = environmentCause.CauseId,
                    Name = "City Reforestation Initiative",
                    ImageUrl = "https://via.placeholder.com/800x400?text=Reforestation",
                    Description = "Planting 10,000 trees in urban areas to combat pollution and climate change.",
                    StartTime = DateTimeOffset.UtcNow.AddDays(-5),
                    EndTime = DateTimeOffset.UtcNow.AddDays(30),
                    MaxDonation = 20000m,
                    Location = "Metro City"
                });
            }

            if (healthCause != null && healthFirst != null)
            {
                await context.WelfareProgrammes.AddAsync(new WelfareProgramme
                {
                    ProgrammeId = Guid.CreateVersion7(),
                    NgoId = healthFirst.NgoId,
                    CauseId = healthCause.CauseId,
                    Name = "Free Mobile Clinic",
                    ImageUrl = "https://via.placeholder.com/800x400?text=Mobile+Clinic",
                    Description = "Deploying mobile clinics to remote villages for free health checkups and basic treatments.",
                    StartTime = DateTimeOffset.UtcNow,
                    MaxDonation = 100000m,
                    Location = "Remote Villages"
                });
            }
            
            await context.SaveChangesAsync();
        }

        // 6. Seed NGO Partners
        if (!await context.NgoPartners.AnyAsync())
        {
            var saveTheChildren = await context.Ngos.FirstOrDefaultAsync(n => n.Name == "Save The Children");
            var greenEarth = await context.Ngos.FirstOrDefaultAsync(n => n.Name == "Green Earth Foundation");
            var techCorp = await context.CorporatePartners.FirstOrDefaultAsync(p => p.Name == "Tech Corp");
            var globalBank = await context.CorporatePartners.FirstOrDefaultAsync(p => p.Name == "Global Bank");

            if (saveTheChildren != null && techCorp != null)
            {
                await context.NgoPartners.AddAsync(new NgoPartner
                {
                    NgoId = saveTheChildren.NgoId,
                    PartnerId = techCorp.PartnerId
                });
            }

            if (greenEarth != null && globalBank != null)
            {
                await context.NgoPartners.AddAsync(new NgoPartner
                {
                    NgoId = greenEarth.NgoId,
                    PartnerId = globalBank.PartnerId
                });
            }

            await context.SaveChangesAsync();
        }
    }
}
