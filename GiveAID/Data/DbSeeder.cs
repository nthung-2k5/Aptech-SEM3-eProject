using GiveAID.Models;
using GiveAID.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context, IPasswordService passwordService)
    {
        // ─────────────────────────────────────────────────────────────────────
        // 1. Seed Users
        // ─────────────────────────────────────────────────────────────────────
        if (!await context.Users.AnyAsync())
        {
            var users = new List<User>
            {
                new()
                {
                    UserId      = Guid.CreateVersion7(),
                    FullName    = "Admin User",
                    Email       = "admin@giveaid.com",
                    PasswordHash = passwordService.HashPassword("Admin@123"),
                    DateOfBirth = new DateOnly(1980, 1, 1),
                    Address     = "123 Admin St, City, Country",
                    PhoneNumber = "0987654321",
                    Occupation  = "Administrator",
                    Role        = UserRole.Admin,
                    IsDeleted   = false
                },
                new()
                {
                    UserId      = Guid.CreateVersion7(),
                    FullName    = "John Doe",
                    Email       = "john@example.com",
                    PasswordHash = passwordService.HashPassword("Member@123"),
                    DateOfBirth = new DateOnly(1990, 5, 15),
                    Address     = "456 Member Ave, City, Country",
                    PhoneNumber = "0912345678",
                    Occupation  = "Software Engineer",
                    Role        = UserRole.Member,
                    IsDeleted   = false
                },
                new()
                {
                    UserId      = Guid.CreateVersion7(),
                    FullName    = "Jane Smith",
                    Email       = "jane@example.com",
                    PasswordHash = passwordService.HashPassword("Member@123"),
                    DateOfBirth = new DateOnly(1995, 8, 20),
                    Address     = "789 User Blvd, City, Country",
                    PhoneNumber = "0934567890",
                    Occupation  = "Teacher",
                    Role        = UserRole.Member,
                    IsDeleted   = false
                },
                new()
                {
                    UserId      = Guid.CreateVersion7(),
                    FullName    = "Michael Tran",
                    Email       = "michael@example.com",
                    PasswordHash = passwordService.HashPassword("Member@123"),
                    DateOfBirth = new DateOnly(1988, 3, 10),
                    Address     = "321 Oak Road, City, Country",
                    PhoneNumber = "0978123456",
                    Occupation  = "Doctor",
                    Role        = UserRole.Member,
                    IsDeleted   = false
                },
                new()
                {
                    UserId      = Guid.CreateVersion7(),
                    FullName    = "Emily Nguyen",
                    Email       = "emily@example.com",
                    PasswordHash = passwordService.HashPassword("Member@123"),
                    DateOfBirth = new DateOnly(1993, 11, 25),
                    Address     = "654 Pine St, City, Country",
                    PhoneNumber = "0956789012",
                    Occupation  = "Designer",
                    Role        = UserRole.Member,
                    IsDeleted   = false
                }
            };
            await context.Users.AddRangeAsync(users);
        }

        // ─────────────────────────────────────────────────────────────────────
        // 2. Seed Donation Causes
        // ─────────────────────────────────────────────────────────────────────
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

        // ─────────────────────────────────────────────────────────────────────
        // 3. Seed NGOs
        // ─────────────────────────────────────────────────────────────────────
        if (!await context.Ngos.AnyAsync())
        {
            var ngos = new List<Ngo>
            {
                new()
                {
                    NgoId       = Guid.CreateVersion7(),
                    Name        = "Save The Children",
                    Description = "Working to improve the lives of children through better education, health care, and economic opportunities.",
                    Address     = "100 Charity Lane, NGO City",
                    PhoneNumber = "0900000001",
                    Website     = "https://savethechildren.org"
                },
                new()
                {
                    NgoId       = Guid.CreateVersion7(),
                    Name        = "Green Earth Foundation",
                    Description = "Dedicated to environmental conservation and fighting climate change.",
                    Address     = "200 Green St, Eco Town",
                    PhoneNumber = "0900000002",
                    Website     = "https://greenearth.org"
                },
                new()
                {
                    NgoId       = Guid.CreateVersion7(),
                    Name        = "Health First Aid",
                    Description = "Providing medical assistance and health education to underprivileged communities.",
                    Address     = "300 Medical Ave, Health City",
                    PhoneNumber = "0900000003",
                    Website     = "https://healthfirst.org"
                }
            };
            await context.Ngos.AddRangeAsync(ngos);
        }

        await context.SaveChangesAsync();

        // ─────────────────────────────────────────────────────────────────────
        // 4. Seed Corporate Partners
        // ─────────────────────────────────────────────────────────────────────
        if (!await context.CorporatePartners.AnyAsync())
        {
            var partners = new List<CorporatePartner>
            {
                new()
                {
                    PartnerId   = Guid.CreateVersion7(),
                    Name        = "Tech Corp",
                    Description = "A leading technology company supporting digital education.",
                    LogoUrl     = "https://placehold.co/150x150?text=TechCorp",
                    WebsiteLink = "https://techcorp.example.com"
                },
                new()
                {
                    PartnerId   = Guid.CreateVersion7(),
                    Name        = "Global Bank",
                    Description = "Financial institution committed to community development.",
                    LogoUrl     = "https://placehold.co/150x150?text=GlobalBank",
                    WebsiteLink = "https://globalbank.example.com"
                },
                new()
                {
                    PartnerId   = Guid.CreateVersion7(),
                    Name        = "EcoVentures",
                    Description = "Sustainable investment firm focused on green initiatives.",
                    LogoUrl     = "https://placehold.co/150x150?text=EcoVentures",
                    WebsiteLink = "https://ecoventures.example.com"
                }
            };
            await context.CorporatePartners.AddRangeAsync(partners);
        }

        await context.SaveChangesAsync();

        // ─────────────────────────────────────────────────────────────────────
        // 5. Seed Welfare Programmes
        // ─────────────────────────────────────────────────────────────────────
        if (!await context.WelfareProgrammes.AnyAsync())
        {
            var educationCause   = await context.DonationCauses.FirstOrDefaultAsync(c => c.Name == "Education");
            var healthCause      = await context.DonationCauses.FirstOrDefaultAsync(c => c.Name == "Healthcare");
            var environmentCause = await context.DonationCauses.FirstOrDefaultAsync(c => c.Name == "Environment");
            var disasterCause    = await context.DonationCauses.FirstOrDefaultAsync(c => c.Name == "Disaster Relief");

            var saveTheChildren = await context.Ngos.FirstOrDefaultAsync(n => n.Name == "Save The Children");
            var greenEarth      = await context.Ngos.FirstOrDefaultAsync(n => n.Name == "Green Earth Foundation");
            var healthFirst     = await context.Ngos.FirstOrDefaultAsync(n => n.Name == "Health First Aid");

            if (educationCause != null && saveTheChildren != null)
            {
                await context.WelfareProgrammes.AddAsync(new WelfareProgramme
                {
                    ProgrammeId = Guid.CreateVersion7(),
                    NgoId       = saveTheChildren.NgoId,
                    CauseId     = educationCause.CauseId,
                    Name        = "Rural School Building Project",
                    ImageUrl    = "https://placehold.co/800x400?text=School+Building",
                    Description = "Building schools in rural areas to provide access to quality education for thousands of children who currently lack access to a proper learning environment.",
                    StartTime   = DateTimeOffset.UtcNow.AddDays(-60),
                    EndTime     = DateTimeOffset.UtcNow.AddDays(100),
                    MaxDonation = 50000m,
                    Location    = "Rural District, Countryside"
                });

                await context.WelfareProgrammes.AddAsync(new WelfareProgramme
                {
                    ProgrammeId = Guid.CreateVersion7(),
                    NgoId       = saveTheChildren.NgoId,
                    CauseId     = educationCause.CauseId,
                    Name        = "Digital Literacy for Youth",
                    ImageUrl    = "https://placehold.co/800x400?text=Digital+Literacy",
                    Description = "Equipping underserved youth with computer skills, internet access, and digital tools to prepare them for the modern workforce.",
                    StartTime   = DateTimeOffset.UtcNow.AddDays(-10),
                    EndTime     = DateTimeOffset.UtcNow.AddDays(200),
                    MaxDonation = 30000m,
                    Location    = "Urban Schools, Metro City"
                });
            }

            if (environmentCause != null && greenEarth != null)
            {
                await context.WelfareProgrammes.AddAsync(new WelfareProgramme
                {
                    ProgrammeId = Guid.CreateVersion7(),
                    NgoId       = greenEarth.NgoId,
                    CauseId     = environmentCause.CauseId,
                    Name        = "City Reforestation Initiative",
                    ImageUrl    = "https://placehold.co/800x400?text=Reforestation",
                    Description = "Planting 10,000 trees in urban areas to combat pollution, restore biodiversity, and fight climate change across the metropolitan region.",
                    StartTime   = DateTimeOffset.UtcNow.AddDays(-5),
                    EndTime     = DateTimeOffset.UtcNow.AddDays(30),
                    MaxDonation = 20000m,
                    Location    = "Metro City"
                });
            }

            if (healthCause != null && healthFirst != null)
            {
                await context.WelfareProgrammes.AddAsync(new WelfareProgramme
                {
                    ProgrammeId = Guid.CreateVersion7(),
                    NgoId       = healthFirst.NgoId,
                    CauseId     = healthCause.CauseId,
                    Name        = "Free Mobile Clinic",
                    ImageUrl    = "https://placehold.co/800x400?text=Mobile+Clinic",
                    Description = "Deploying mobile clinics to remote villages for free health checkups, vaccinations, and basic treatments reaching thousands of unserved patients.",
                    StartTime   = DateTimeOffset.UtcNow,
                    MaxDonation = 100000m,
                    Location    = "Remote Villages"
                });
            }

            if (disasterCause != null && healthFirst != null)
            {
                await context.WelfareProgrammes.AddAsync(new WelfareProgramme
                {
                    ProgrammeId = Guid.CreateVersion7(),
                    NgoId       = healthFirst.NgoId,
                    CauseId     = disasterCause.CauseId,
                    Name        = "Flood Relief Fund",
                    ImageUrl    = "https://placehold.co/800x400?text=Flood+Relief",
                    Description = "Emergency relief supplies, temporary shelter, and medical aid for families displaced by recent flooding in the northern provinces.",
                    StartTime   = DateTimeOffset.UtcNow.AddDays(-3),
                    EndTime     = DateTimeOffset.UtcNow.AddDays(60),
                    MaxDonation = 75000m,
                    Location    = "Northern Provinces"
                });
            }

            await context.SaveChangesAsync();
        }

        // ─────────────────────────────────────────────────────────────────────
        // 6. Seed Gallery Images
        // ─────────────────────────────────────────────────────────────────────
        if (!await context.GalleryImages.AnyAsync())
        {
            var schoolProgramme   = await context.WelfareProgrammes.FirstOrDefaultAsync(p => p.Name == "Rural School Building Project");
            var reforestProgramme = await context.WelfareProgrammes.FirstOrDefaultAsync(p => p.Name == "City Reforestation Initiative");
            var clinicProgramme   = await context.WelfareProgrammes.FirstOrDefaultAsync(p => p.Name == "Free Mobile Clinic");
            var floodProgramme    = await context.WelfareProgrammes.FirstOrDefaultAsync(p => p.Name == "Flood Relief Fund");

            var galleryImages = new List<GalleryImage>();

            if (schoolProgramme != null)
            {
                galleryImages.AddRange(
                [
                    new GalleryImage { ImageId = Guid.CreateVersion7(), ProgrammeId = schoolProgramme.ProgrammeId, ImageUrl = "https://placehold.co/800x600?text=School+Foundation", Caption = "Laying the foundation of the new school" },
                    new GalleryImage { ImageId = Guid.CreateVersion7(), ProgrammeId = schoolProgramme.ProgrammeId, ImageUrl = "https://placehold.co/800x600?text=School+Walls+Up",    Caption = "Walls going up in record time" },
                    new GalleryImage { ImageId = Guid.CreateVersion7(), ProgrammeId = schoolProgramme.ProgrammeId, ImageUrl = "https://placehold.co/800x600?text=Children+Learning",  Caption = "Children attending their first classes" }
                ]);
            }

            if (reforestProgramme != null)
            {
                galleryImages.AddRange(
                [
                    new GalleryImage { ImageId = Guid.CreateVersion7(), ProgrammeId = reforestProgramme.ProgrammeId, ImageUrl = "https://placehold.co/800x600?text=Tree+Planting+Day", Caption = "Community tree planting event" },
                    new GalleryImage { ImageId = Guid.CreateVersion7(), ProgrammeId = reforestProgramme.ProgrammeId, ImageUrl = "https://placehold.co/800x600?text=Green+Saplings",     Caption = "Young saplings ready for planting" }
                ]);
            }

            if (clinicProgramme != null)
            {
                galleryImages.AddRange(
                [
                    new GalleryImage { ImageId = Guid.CreateVersion7(), ProgrammeId = clinicProgramme.ProgrammeId, ImageUrl = "https://placehold.co/800x600?text=Mobile+Clinic+Arrival", Caption = "Mobile clinic arriving at a remote village" },
                    new GalleryImage { ImageId = Guid.CreateVersion7(), ProgrammeId = clinicProgramme.ProgrammeId, ImageUrl = "https://placehold.co/800x600?text=Health+Checkup",         Caption = "Doctors conducting free health screenings" }
                ]);
            }

            if (floodProgramme != null)
            {
                galleryImages.AddRange(
                [
                    new GalleryImage { ImageId = Guid.CreateVersion7(), ProgrammeId = floodProgramme.ProgrammeId, ImageUrl = "https://placehold.co/800x600?text=Relief+Supplies",   Caption = "Relief supplies being distributed" },
                    new GalleryImage { ImageId = Guid.CreateVersion7(), ProgrammeId = floodProgramme.ProgrammeId, ImageUrl = "https://placehold.co/800x600?text=Temporary+Shelter", Caption = "Temporary shelters set up for displaced families" }
                ]);
            }

            // Standalone gallery images not tied to a programme
            galleryImages.AddRange(
            [
                new GalleryImage { ImageId = Guid.CreateVersion7(), ProgrammeId = null, ImageUrl = "https://placehold.co/800x600?text=Volunteer+Day",     Caption = "Annual volunteer appreciation day" },
                new GalleryImage { ImageId = Guid.CreateVersion7(), ProgrammeId = null, ImageUrl = "https://placehold.co/800x600?text=Fundraising+Gala",  Caption = "Annual charity fundraising gala" }
            ]);

            await context.GalleryImages.AddRangeAsync(galleryImages);
            await context.SaveChangesAsync();
        }

        // ─────────────────────────────────────────────────────────────────────
        // 7. Seed NGO Partners
        // ─────────────────────────────────────────────────────────────────────
        if (!await context.NgoPartners.AnyAsync())
        {
            var saveTheChildren = await context.Ngos.FirstOrDefaultAsync(n => n.Name == "Save The Children");
            var greenEarth      = await context.Ngos.FirstOrDefaultAsync(n => n.Name == "Green Earth Foundation");
            var healthFirst     = await context.Ngos.FirstOrDefaultAsync(n => n.Name == "Health First Aid");
            var techCorp        = await context.CorporatePartners.FirstOrDefaultAsync(p => p.Name == "Tech Corp");
            var globalBank      = await context.CorporatePartners.FirstOrDefaultAsync(p => p.Name == "Global Bank");
            var ecoVentures     = await context.CorporatePartners.FirstOrDefaultAsync(p => p.Name == "EcoVentures");

            var ngoPartners = new List<NgoPartner>();

            if (saveTheChildren != null && techCorp    != null) ngoPartners.Add(new NgoPartner { NgoId = saveTheChildren.NgoId, PartnerId = techCorp.PartnerId });
            if (saveTheChildren != null && globalBank  != null) ngoPartners.Add(new NgoPartner { NgoId = saveTheChildren.NgoId, PartnerId = globalBank.PartnerId });
            if (greenEarth      != null && globalBank  != null) ngoPartners.Add(new NgoPartner { NgoId = greenEarth.NgoId,      PartnerId = globalBank.PartnerId });
            if (greenEarth      != null && ecoVentures != null) ngoPartners.Add(new NgoPartner { NgoId = greenEarth.NgoId,      PartnerId = ecoVentures.PartnerId });
            if (healthFirst     != null && techCorp    != null) ngoPartners.Add(new NgoPartner { NgoId = healthFirst.NgoId,     PartnerId = techCorp.PartnerId });

            if (ngoPartners.Count > 0)
                await context.NgoPartners.AddRangeAsync(ngoPartners);

            await context.SaveChangesAsync();
        }

        // ─────────────────────────────────────────────────────────────────────
        // 8. Seed User Interests
        // ─────────────────────────────────────────────────────────────────────
        if (!await context.UserInterests.AnyAsync())
        {
            var john    = await context.Users.FirstOrDefaultAsync(u => u.Email == "john@example.com");
            var jane    = await context.Users.FirstOrDefaultAsync(u => u.Email == "jane@example.com");
            var michael = await context.Users.FirstOrDefaultAsync(u => u.Email == "michael@example.com");
            var emily   = await context.Users.FirstOrDefaultAsync(u => u.Email == "emily@example.com");

            var saveTheChildren = await context.Ngos.FirstOrDefaultAsync(n => n.Name == "Save The Children");
            var greenEarth      = await context.Ngos.FirstOrDefaultAsync(n => n.Name == "Green Earth Foundation");
            var healthFirst     = await context.Ngos.FirstOrDefaultAsync(n => n.Name == "Health First Aid");

            var interests = new List<UserInterest>();

            if (john    != null && saveTheChildren != null) interests.Add(new UserInterest { UserId = john.UserId,    NgoId = saveTheChildren.NgoId });
            if (john    != null && greenEarth      != null) interests.Add(new UserInterest { UserId = john.UserId,    NgoId = greenEarth.NgoId });
            if (jane    != null && saveTheChildren != null) interests.Add(new UserInterest { UserId = jane.UserId,    NgoId = saveTheChildren.NgoId });
            if (jane    != null && healthFirst     != null) interests.Add(new UserInterest { UserId = jane.UserId,    NgoId = healthFirst.NgoId });
            if (michael != null && healthFirst     != null) interests.Add(new UserInterest { UserId = michael.UserId, NgoId = healthFirst.NgoId });
            if (emily   != null && greenEarth      != null) interests.Add(new UserInterest { UserId = emily.UserId,   NgoId = greenEarth.NgoId });
            if (emily   != null && saveTheChildren != null) interests.Add(new UserInterest { UserId = emily.UserId,   NgoId = saveTheChildren.NgoId });

            if (interests.Count > 0)
                await context.UserInterests.AddRangeAsync(interests);

            await context.SaveChangesAsync();
        }

        // ─────────────────────────────────────────────────────────────────────
        // 9. Seed Transactions & Donations
        // ─────────────────────────────────────────────────────────────────────
        if (!await context.Transactions.AnyAsync())
        {
            var john    = await context.Users.FirstOrDefaultAsync(u => u.Email == "john@example.com");
            var jane    = await context.Users.FirstOrDefaultAsync(u => u.Email == "jane@example.com");
            var michael = await context.Users.FirstOrDefaultAsync(u => u.Email == "michael@example.com");
            var emily   = await context.Users.FirstOrDefaultAsync(u => u.Email == "emily@example.com");

            var saveTheChildren   = await context.Ngos.FirstOrDefaultAsync(n => n.Name == "Save The Children");
            var greenEarth        = await context.Ngos.FirstOrDefaultAsync(n => n.Name == "Green Earth Foundation");
            var healthFirst       = await context.Ngos.FirstOrDefaultAsync(n => n.Name == "Health First Aid");

            var schoolProgramme   = await context.WelfareProgrammes.FirstOrDefaultAsync(p => p.Name == "Rural School Building Project");
            var reforestProgramme = await context.WelfareProgrammes.FirstOrDefaultAsync(p => p.Name == "City Reforestation Initiative");
            var clinicProgramme   = await context.WelfareProgrammes.FirstOrDefaultAsync(p => p.Name == "Free Mobile Clinic");
            var floodProgramme    = await context.WelfareProgrammes.FirstOrDefaultAsync(p => p.Name == "Flood Relief Fund");

            var educationCause    = await context.DonationCauses.FirstOrDefaultAsync(c => c.Name == "Education");
            var environmentCause  = await context.DonationCauses.FirstOrDefaultAsync(c => c.Name == "Environment");
            var healthCause       = await context.DonationCauses.FirstOrDefaultAsync(c => c.Name == "Healthcare");
            var disasterCause     = await context.DonationCauses.FirstOrDefaultAsync(c => c.Name == "Disaster Relief");

            var transactions = new List<Transaction>();
            var donations    = new List<Donation>();

            void AddDonation(
                User? donor, Ngo? ngo, WelfareProgramme? programme, DonationCause? cause,
                decimal amount, string gateway, string account, string content, string refCode,
                DateTimeOffset time, DonationStatus status = DonationStatus.Completed)
            {
                if (donor == null) return;
                var txId = Guid.CreateVersion7();
                transactions.Add(new Transaction
                {
                    TransactionId   = txId,
                    Gateway         = gateway,
                    AccountNumber   = account,
                    Content         = content,
                    Amount          = amount,
                    ReferenceCode   = refCode,
                    TransactionTime = time
                });
                donations.Add(new Donation
                {
                    DonationId    = Guid.CreateVersion7(),
                    UserId        = donor.UserId,
                    NgoId         = ngo?.NgoId,
                    ProgrammeId   = programme?.ProgrammeId,
                    CauseId       = cause?.CauseId,
                    TransactionId = txId,
                    Amount        = amount,
                    Status        = status,
                    CreatedAt     = time
                });
            }

            // John's donations
            AddDonation(john, saveTheChildren, schoolProgramme,   educationCause,   500m,  "VNPay",   "9704123456789012", "Donate school project",    "TXN-JD-001", DateTimeOffset.UtcNow.AddDays(-45));
            AddDonation(john, greenEarth,      reforestProgramme, environmentCause, 300m,  "MoMo",    "0912345678",       "Plant trees for future",   "TXN-JD-002", DateTimeOffset.UtcNow.AddDays(-30));
            AddDonation(john, healthFirst,     clinicProgramme,   healthCause,      750m,  "VNPay",   "9704123456789012", "Support mobile clinic",    "TXN-JD-003", DateTimeOffset.UtcNow.AddDays(-10));

            // Jane's donations
            AddDonation(jane, saveTheChildren, schoolProgramme,   educationCause,   1000m, "ZaloPay", "0934567890",       "Education for all",        "TXN-JS-001", DateTimeOffset.UtcNow.AddDays(-50));
            AddDonation(jane, healthFirst,     floodProgramme,    disasterCause,    250m,  "MoMo",    "0934567890",       "Flood relief support",     "TXN-JS-002", DateTimeOffset.UtcNow.AddDays(-2));
            AddDonation(jane, greenEarth,      reforestProgramme, environmentCause, 200m,  "VNPay",   "9704098765432100", "Reforestation donation",   "TXN-JS-003", DateTimeOffset.UtcNow.AddDays(-15), DonationStatus.Void);

            // Michael's donations
            AddDonation(michael, healthFirst,  clinicProgramme,   healthCause,      2000m, "VNPay",   "9704111122223333", "Medical support donation", "TXN-MT-001", DateTimeOffset.UtcNow.AddDays(-20));
            AddDonation(michael, healthFirst,  floodProgramme,    disasterCause,    500m,  "ZaloPay", "0978123456",       "Emergency flood aid",      "TXN-MT-002", DateTimeOffset.UtcNow.AddDays(-1));

            // Emily's donations
            AddDonation(emily, greenEarth,     reforestProgramme, environmentCause, 800m,  "MoMo",    "0956789012",       "Go green initiative",      "TXN-EN-001", DateTimeOffset.UtcNow.AddDays(-7));
            AddDonation(emily, saveTheChildren, schoolProgramme,  educationCause,   400m,  "VNPay",   "9704444455556666", "Kids deserve a future",    "TXN-EN-002", DateTimeOffset.UtcNow.AddDays(-3));

            await context.Transactions.AddRangeAsync(transactions);
            await context.Donations.AddRangeAsync(donations);
            await context.SaveChangesAsync();
        }

        // ─────────────────────────────────────────────────────────────────────
        // 10. Seed Notifications
        // ─────────────────────────────────────────────────────────────────────
        if (!await context.Notifications.AnyAsync())
        {
            var john    = await context.Users.FirstOrDefaultAsync(u => u.Email == "john@example.com");
            var jane    = await context.Users.FirstOrDefaultAsync(u => u.Email == "jane@example.com");
            var michael = await context.Users.FirstOrDefaultAsync(u => u.Email == "michael@example.com");
            var emily   = await context.Users.FirstOrDefaultAsync(u => u.Email == "emily@example.com");

            var notifications = new List<Notification>();

            if (john != null)
            {
                notifications.AddRange(
                [
                    new Notification { NotificationId = Guid.CreateVersion7(), UserId = john.UserId, Content = "Your donation of $500 to 'Rural School Building Project' was received. Thank you!", IsRead = true,  CreatedAt = DateTimeOffset.UtcNow.AddDays(-45) },
                    new Notification { NotificationId = Guid.CreateVersion7(), UserId = john.UserId, Content = "Your donation of $300 to 'City Reforestation Initiative' was received. Thank you!", IsRead = true,  CreatedAt = DateTimeOffset.UtcNow.AddDays(-30) },
                    new Notification { NotificationId = Guid.CreateVersion7(), UserId = john.UserId, Content = "New welfare programme 'Flood Relief Fund' has launched. Consider donating!",       IsRead = false, CreatedAt = DateTimeOffset.UtcNow.AddDays(-3) }
                ]);
            }

            if (jane != null)
            {
                notifications.AddRange(
                [
                    new Notification { NotificationId = Guid.CreateVersion7(), UserId = jane.UserId, Content = "Your donation of $1000 to 'Rural School Building Project' was received. Thank you!", IsRead = true,  CreatedAt = DateTimeOffset.UtcNow.AddDays(-50) },
                    new Notification { NotificationId = Guid.CreateVersion7(), UserId = jane.UserId, Content = "Your donation of $200 was voided due to a payment issue. Please try again.",        IsRead = false, CreatedAt = DateTimeOffset.UtcNow.AddDays(-15) },
                    new Notification { NotificationId = Guid.CreateVersion7(), UserId = jane.UserId, Content = "Your donation of $250 to 'Flood Relief Fund' was received. Thank you!",            IsRead = false, CreatedAt = DateTimeOffset.UtcNow.AddDays(-2) }
                ]);
            }

            if (michael != null)
            {
                notifications.AddRange(
                [
                    new Notification { NotificationId = Guid.CreateVersion7(), UserId = michael.UserId, Content = "Your generous donation of $2000 to 'Free Mobile Clinic' was received. Thank you!", IsRead = true,  CreatedAt = DateTimeOffset.UtcNow.AddDays(-20) },
                    new Notification { NotificationId = Guid.CreateVersion7(), UserId = michael.UserId, Content = "Your donation of $500 to 'Flood Relief Fund' was received. Thank you!",            IsRead = false, CreatedAt = DateTimeOffset.UtcNow.AddDays(-1) }
                ]);
            }

            if (emily != null)
            {
                notifications.AddRange(
                [
                    new Notification { NotificationId = Guid.CreateVersion7(), UserId = emily.UserId, Content = "Your donation of $800 to 'City Reforestation Initiative' was received. Thank you!", IsRead = true,  CreatedAt = DateTimeOffset.UtcNow.AddDays(-7) },
                    new Notification { NotificationId = Guid.CreateVersion7(), UserId = emily.UserId, Content = "Your donation of $400 to 'Rural School Building Project' was received. Thank you!", IsRead = false, CreatedAt = DateTimeOffset.UtcNow.AddDays(-3) }
                ]);
            }

            if (notifications.Count > 0)
                await context.Notifications.AddRangeAsync(notifications);

            await context.SaveChangesAsync();
        }

        // ─────────────────────────────────────────────────────────────────────
        // 11. Seed User Queries
        // ─────────────────────────────────────────────────────────────────────
        if (!await context.UserQueries.AnyAsync())
        {
            var john    = await context.Users.FirstOrDefaultAsync(u => u.Email == "john@example.com");
            var jane    = await context.Users.FirstOrDefaultAsync(u => u.Email == "jane@example.com");
            var michael = await context.Users.FirstOrDefaultAsync(u => u.Email == "michael@example.com");

            var queries = new List<UserQuery>();

            if (john != null)
            {
                queries.AddRange(
                [
                    new UserQuery
                    {
                        QueryId     = Guid.CreateVersion7(),
                        UserId      = john.UserId,
                        Subject     = "How do I get a donation receipt?",
                        MessageText = "Hi, I made a donation last week to the school building project. Can I get an official receipt for tax purposes?",
                        ReplyText   = "Hi John! Thank you for your generous donation. You can download your receipt from My Donations > View > Download Receipt. Let us know if you need further assistance!",
                        CreatedAt   = DateTimeOffset.UtcNow.AddDays(-40)
                    },
                    new UserQuery
                    {
                        QueryId     = Guid.CreateVersion7(),
                        UserId      = john.UserId,
                        Subject     = "Can I set up recurring donations?",
                        MessageText = "Is there a way to set up a monthly recurring donation to a welfare programme?",
                        ReplyText   = null,
                        CreatedAt   = DateTimeOffset.UtcNow.AddDays(-5)
                    }
                ]);
            }

            if (jane != null)
            {
                queries.Add(new UserQuery
                {
                    QueryId     = Guid.CreateVersion7(),
                    UserId      = jane.UserId,
                    Subject     = "Voided donation - need help",
                    MessageText = "My donation of $200 was voided. I was charged but the donation did not go through. Please help me resolve this.",
                    ReplyText   = "Hi Jane! We sincerely apologize for the inconvenience. Our team has investigated and found a temporary payment gateway issue. A full refund has been initiated and will reflect in 3-5 business days.",
                    CreatedAt   = DateTimeOffset.UtcNow.AddDays(-14)
                });
            }

            if (michael != null)
            {
                queries.Add(new UserQuery
                {
                    QueryId     = Guid.CreateVersion7(),
                    UserId      = michael.UserId,
                    Subject     = "Partnership opportunities",
                    MessageText = "I work for a hospital and we're interested in partnering with Health First Aid. Who should I contact?",
                    ReplyText   = "Hi Michael! We love hearing from healthcare professionals. Please email partnerships@giveaid.com with your organization details and our team will reach out within 48 hours.",
                    CreatedAt   = DateTimeOffset.UtcNow.AddDays(-18)
                });
            }

            if (queries.Count > 0)
                await context.UserQueries.AddRangeAsync(queries);

            await context.SaveChangesAsync();
        }

        // ─────────────────────────────────────────────────────────────────────
        // 12. Seed About Us Subpages & User Modifications
        // ─────────────────────────────────────────────────────────────────────
        if (!await context.AboutUsSubpages.AnyAsync())
        {
            var admin = await context.Users.FirstOrDefaultAsync(u => u.Email == "admin@giveaid.com");

            var subpages = new List<AboutUsSubpage>
            {
                new() { SubpageId = Guid.CreateVersion7(), Slug = "our-mission", Title = "Our Mission" },
                new() { SubpageId = Guid.CreateVersion7(), Slug = "our-story",   Title = "Our Story"   },
                new() { SubpageId = Guid.CreateVersion7(), Slug = "our-team",    Title = "Our Team"    }
            };

            await context.AboutUsSubpages.AddRangeAsync(subpages);
            await context.SaveChangesAsync();

            // Each subpage requires at least one UserModification (HtmlContent is read from it)
            var modifications = new List<UserModification>
            {
                new()
                {
                    ModificationId = Guid.CreateVersion7(),
                    SubpageId      = subpages[0].SubpageId,
                    UserId         = admin?.UserId,
                    HtmlContent    =
                        "<section class=\"mission-section\">" +
                        "<h2>What Drives Us</h2>" +
                        "<p>At GiveAID, our mission is to connect compassionate donors with life-changing welfare programmes run by trusted NGOs around the world. We believe every contribution, big or small, has the power to transform lives.</p>" +
                        "<h2>Our Commitment</h2>" +
                        "<p>We are committed to transparency, accountability, and measurable impact. Every donation is tracked, and donors receive regular updates on how their funds are being used to create real change.</p>" +
                        "</section>",
                    CreatedAt = DateTimeOffset.UtcNow.AddDays(-90)
                },
                new()
                {
                    ModificationId = Guid.CreateVersion7(),
                    SubpageId      = subpages[1].SubpageId,
                    UserId         = admin?.UserId,
                    HtmlContent    =
                        "<section class=\"story-section\">" +
                        "<h2>How It All Started</h2>" +
                        "<p>GiveAID was founded in 2018 by a group of volunteers who saw the disconnect between people who wanted to help and the organisations that needed support. We built a platform to bridge that gap.</p>" +
                        "<h2>Where We Are Today</h2>" +
                        "<p>Today, GiveAID partners with dozens of NGOs across multiple countries, facilitating millions in donations and supporting thousands of welfare programmes that change lives every single day.</p>" +
                        "</section>",
                    CreatedAt = DateTimeOffset.UtcNow.AddDays(-90)
                },
                new()
                {
                    ModificationId = Guid.CreateVersion7(),
                    SubpageId      = subpages[2].SubpageId,
                    UserId         = admin?.UserId,
                    HtmlContent    =
                        "<section class=\"team-section\">" +
                        "<h2>The People Behind GiveAID</h2>" +
                        "<p>Our team is made up of passionate individuals from diverse backgrounds — technology, social work, finance, and community development — all united by a single goal: to make giving easier and more impactful.</p>" +
                        "<h2>Join Us</h2>" +
                        "<p>We are always looking for volunteers, partners, and supporters. If you share our vision of a more generous world, <a href=\"/contact\">get in touch</a> with us today.</p>" +
                        "</section>",
                    CreatedAt = DateTimeOffset.UtcNow.AddDays(-90)
                }
            };

            await context.UserModifications.AddRangeAsync(modifications);
            await context.SaveChangesAsync();
        }
    }
}
