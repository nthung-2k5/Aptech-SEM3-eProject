using GiveAID.Models;
using GiveAID.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context, IPasswordService passwordService)
    {
        var now = DateTimeOffset.UtcNow;
        var nowDate = DateOnly.FromDateTime(now.UtcDateTime);

        // ─────────────────────────────────────────────────────────────────────
        // 1. USERS
        // ─────────────────────────────────────────────────────────────────────
        if (!await context.Users.AnyAsync())
        {
            var adminHash = passwordService.HashPassword("Admin@123");
            var memberHash = passwordService.HashPassword("Member@123");

            var users = new List<User>
            {
                // Admin
                new() { UserId = Guid.CreateVersion7(), FullName = "Admin GiveAID",        Email = "admin@giveaid.com",           PasswordHash = adminHash,  DateOfBirth = new DateOnly(1980, 1,  1),  Address = "10 Nguyen Hue Blvd, Ho Chi Minh City", PhoneNumber = "0900000000", Occupation = "Platform Administrator", Role = UserRole.Admin,  IsDeleted = false, CreatedAt = now.AddMonths(-12) },
                // Members
                new() { UserId = Guid.CreateVersion7(), FullName = "Nguyen Van An",        Email = "an.nguyen@example.com",       PasswordHash = memberHash, DateOfBirth = new DateOnly(1990, 3, 15),  Address = "12 Le Loi St, Hanoi",                  PhoneNumber = "0901110001", Occupation = "Software Engineer",      Role = UserRole.Member, IsDeleted = false, CreatedAt = now.AddMonths(-5).AddDays(-10) },
                new() { UserId = Guid.CreateVersion7(), FullName = "Tran Thi Bich Ngoc",   Email = "bich.ngoc@example.com",       PasswordHash = memberHash, DateOfBirth = new DateOnly(1993, 7, 22),  Address = "45 Tran Hung Dao, Da Nang",            PhoneNumber = "0901110002", Occupation = "Nurse",                  Role = UserRole.Member, IsDeleted = false, CreatedAt = now.AddMonths(-5).AddDays(-2) },
                new() { UserId = Guid.CreateVersion7(), FullName = "Le Minh Duc",          Email = "duc.le@example.com",          PasswordHash = memberHash, DateOfBirth = new DateOnly(1988, 11, 3),  Address = "78 Hai Ba Trung, Ho Chi Minh City",    PhoneNumber = "0901110003", Occupation = "Doctor",                 Role = UserRole.Member, IsDeleted = false, CreatedAt = now.AddMonths(-4).AddDays(-15) },
                new() { UserId = Guid.CreateVersion7(), FullName = "Pham Thu Ha",          Email = "thu.ha@example.com",          PasswordHash = memberHash, DateOfBirth = new DateOnly(1995, 5, 18),  Address = "3 Nguyen Du, Hue City",                PhoneNumber = "0901110004", Occupation = "Teacher",                Role = UserRole.Member, IsDeleted = false, CreatedAt = now.AddMonths(-4).AddDays(-5) },
                new() { UserId = Guid.CreateVersion7(), FullName = "Hoang Quoc Khanh",     Email = "khanh.hoang@example.com",     PasswordHash = memberHash, DateOfBirth = new DateOnly(1985, 9, 30),  Address = "22 Bach Dang, Hai Phong",              PhoneNumber = "0901110005", Occupation = "Accountant",             Role = UserRole.Member, IsDeleted = false, CreatedAt = now.AddMonths(-3).AddDays(-10) },
                new() { UserId = Guid.CreateVersion7(), FullName = "Vu Thi Lan Anh",       Email = "lan.anh@example.com",         PasswordHash = memberHash, DateOfBirth = new DateOnly(1997, 2, 14),  Address = "9 Phan Chu Trinh, Can Tho",            PhoneNumber = "0901110006", Occupation = "University Student",     Role = UserRole.Member, IsDeleted = false, CreatedAt = now.AddMonths(-3).AddDays(5) },
                new() { UserId = Guid.CreateVersion7(), FullName = "Dang Van Minh Khoa",   Email = "minh.khoa@example.com",       PasswordHash = memberHash, DateOfBirth = new DateOnly(1991, 6, 25),  Address = "56 Ly Thuong Kiet, Nha Trang",         PhoneNumber = "0901110007", Occupation = "Architect",              Role = UserRole.Member, IsDeleted = false, CreatedAt = now.AddMonths(-2).AddDays(-8) },
                new() { UserId = Guid.CreateVersion7(), FullName = "Bui Ngoc Nhung",       Email = "ngoc.nhung@example.com",      PasswordHash = memberHash, DateOfBirth = new DateOnly(1989, 4, 8),   Address = "34 Vo Thi Sau, Vung Tau",              PhoneNumber = "0901110008", Occupation = "Journalist",             Role = UserRole.Member, IsDeleted = false, CreatedAt = now.AddMonths(-2).AddDays(4) },
                new() { UserId = Guid.CreateVersion7(), FullName = "Do Thanh Phong",       Email = "thanh.phong@example.com",     PasswordHash = memberHash, DateOfBirth = new DateOnly(1994, 8, 12),  Address = "71 Nguyen Du, Bien Hoa",               PhoneNumber = "0901110009", Occupation = "Lawyer",                 Role = UserRole.Member, IsDeleted = false, CreatedAt = now.AddMonths(-1).AddDays(-10) },
                new() { UserId = Guid.CreateVersion7(), FullName = "Ly Thi Quynh Nhu",     Email = "quynh.nhu@example.com",       PasswordHash = memberHash, DateOfBirth = new DateOnly(1996, 12, 1),  Address = "18 Tran Phu, Quy Nhon",                PhoneNumber = "0901110010", Occupation = "Graphic Designer",       Role = UserRole.Member, IsDeleted = false, CreatedAt = now.AddMonths(-1).AddDays(5) },
            };
            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }

        // ─────────────────────────────────────────────────────────────────────
        // 2. DONATION CAUSES
        // ─────────────────────────────────────────────────────────────────────
        if (!await context.DonationCauses.AnyAsync())
        {
            var causes = new List<DonationCause>
            {
                new() { CauseId = Guid.CreateVersion7(), Name = "Disaster Relief",   IsDeleted = false },
                new() { CauseId = Guid.CreateVersion7(), Name = "Education",          IsDeleted = false },
                new() { CauseId = Guid.CreateVersion7(), Name = "Healthcare",         IsDeleted = false },
                new() { CauseId = Guid.CreateVersion7(), Name = "Environment",        IsDeleted = false },
                new() { CauseId = Guid.CreateVersion7(), Name = "Hunger & Food Aid",  IsDeleted = false },
            };
            await context.DonationCauses.AddRangeAsync(causes);
            await context.SaveChangesAsync();
        }

        // ─────────────────────────────────────────────────────────────────────
        // 3. NGOs  — 5 real globally recognized organisations
        // ─────────────────────────────────────────────────────────────────────
        if (!await context.Ngos.AnyAsync())
        {
            var ngos = new List<Ngo>
            {
                new()
                {
                    NgoId       = Guid.CreateVersion7(),
                    Name        = "UNICEF",
                    Description = "The United Nations Children's Fund works in over 190 countries to save children's lives, defend their rights, and help them fulfil their potential. UNICEF focuses on child survival, education, equality, and emergency response.",
                    Address     = "3 United Nations Plaza, New York, NY 10017, USA",
                    PhoneNumber = "+1-212-326-7000",
                    Website     = "https://www.unicef.org",
                    IsDeleted   = false,
                    CreatedAt   = now.AddMonths(-5).AddDays(-15)
                },
                new()
                {
                    NgoId       = Guid.CreateVersion7(),
                    Name        = "World Wildlife Fund (WWF)",
                    Description = "WWF is the world's largest conservation organisation, working to protect the natural world and tackle the forces that threaten it. Operating in 100+ countries, WWF works to conserve nature and reduce the most pressing threats to the diversity of life on Earth.",
                    Address     = "1250 24th Street NW, Washington, DC 20037, USA",
                    PhoneNumber = "+1-202-293-4800",
                    Website     = "https://www.worldwildlife.org",
                    IsDeleted   = false,
                    CreatedAt   = now.AddMonths(-4).AddDays(5)
                },
                new()
                {
                    NgoId       = Guid.CreateVersion7(),
                    Name        = "Médecins Sans Frontières (MSF)",
                    Description = "Doctors Without Borders delivers emergency medical aid to people affected by conflict, epidemics, disasters, and exclusion from healthcare. Operational in 70+ countries, MSF provides impartial, independent humanitarian assistance.",
                    Address     = "78 Rue de Lausanne, 1202 Geneva, Switzerland",
                    PhoneNumber = "+41-22-849-8400",
                    Website     = "https://www.msf.org",
                    IsDeleted   = false,
                    CreatedAt   = now.AddMonths(-3).AddDays(-2)
                },
                new()
                {
                    NgoId       = Guid.CreateVersion7(),
                    Name        = "International Red Cross (ICRC)",
                    Description = "The International Committee of the Red Cross is an impartial, neutral, and independent humanitarian organisation that protects and assists victims of armed conflict and other situations of violence. It has a permanent mandate under international humanitarian law.",
                    Address     = "19 Avenue de la Paix, 1202 Geneva, Switzerland",
                    PhoneNumber = "+41-22-734-6001",
                    Website     = "https://www.icrc.org",
                    IsDeleted   = false,
                    CreatedAt   = now.AddMonths(-2).AddDays(-10)
                },
                new()
                {
                    NgoId       = Guid.CreateVersion7(),
                    Name        = "Save the Children",
                    Description = "Save the Children gives children a healthy start in life, the opportunity to learn, and protection from harm. Operating in 116 countries, the organisation fights for children's rights and delivers immediate and lasting change to their lives and futures.",
                    Address     = "501 Kings Highway East, Suite 400, Fairfield, CT 06825, USA",
                    PhoneNumber = "+1-203-221-4000",
                    Website     = "https://www.savethechildren.org",
                    IsDeleted   = false,
                    CreatedAt   = now.AddMonths(-1).AddDays(5)
                },
            };
            await context.Ngos.AddRangeAsync(ngos);
            await context.SaveChangesAsync();
        }

        // ─────────────────────────────────────────────────────────────────────
        // 4. CORPORATE PARTNERS
        // ─────────────────────────────────────────────────────────────────────
        if (!await context.CorporatePartners.AnyAsync())
        {
            var partners = new List<CorporatePartner>
            {
                new() { PartnerId = Guid.CreateVersion7(), Name = "Viettel Group",       LogoUrl = "/images/partners-logo/viettel-logo.png",    WebsiteLink = "https://viettel.com.vn" },
                new() { PartnerId = Guid.CreateVersion7(), Name = "Vingroup JSC",        LogoUrl = "/images/partners-logo/vingroup-logo.png",   WebsiteLink = "https://vingroup.net" },
                new() { PartnerId = Guid.CreateVersion7(), Name = "FPT Corporation",     LogoUrl = "/images/partners-logo/fpt-logo.png",        WebsiteLink = "https://fpt.com.vn" },
                new() { PartnerId = Guid.CreateVersion7(), Name = "Masan Group",         LogoUrl = "/images/partners-logo/masan-logo.png",      WebsiteLink = "https://masangroup.com" },
                new() { PartnerId = Guid.CreateVersion7(), Name = "Techcombank",         LogoUrl = "/images/partners-logo/techcombank-logo.png",        WebsiteLink = "https://techcombank.com.vn" },
            };
            await context.CorporatePartners.AddRangeAsync(partners);
            await context.SaveChangesAsync();
        }

        // ─────────────────────────────────────────────────────────────────────
        // 5. WELFARE PROGRAMMES — 10 real / highly realistic global campaigns
        // ─────────────────────────────────────────────────────────────────────
        if (!await context.WelfareProgrammes.AnyAsync())
        {
            // Lookup seeded entities
            var unicef = await context.Ngos.FirstAsync(n => n.Name == "UNICEF");
            var wwf = await context.Ngos.FirstAsync(n => n.Name == "World Wildlife Fund (WWF)");
            var msf = await context.Ngos.FirstAsync(n => n.Name == "Médecins Sans Frontières (MSF)");
            var redCross = await context.Ngos.FirstAsync(n => n.Name == "International Red Cross (ICRC)");
            var saveChildren = await context.Ngos.FirstAsync(n => n.Name == "Save the Children");

            var causeDisaster = await context.DonationCauses.FirstAsync(c => c.Name == "Disaster Relief");
            var causeEducation = await context.DonationCauses.FirstAsync(c => c.Name == "Education");
            var causeHealth = await context.DonationCauses.FirstAsync(c => c.Name == "Healthcare");
            var causeEnv = await context.DonationCauses.FirstAsync(c => c.Name == "Environment");
            var causeHunger = await context.DonationCauses.FirstAsync(c => c.Name == "Hunger & Food Aid");

            var programmes = new List<WelfareProgramme>
            {
                new()
                {
                    ProgrammeId = Guid.CreateVersion7(),
                    NgoId       = redCross.NgoId,
                    CauseId     = causeDisaster.CauseId,
                    Name        = "Turkey-Syria Earthquake Relief 2023",
                    ImageUrl    = "/images/programmes/turkey-syria-earthquake-relief.jpg",
                    Description = "The catastrophic 7.8-magnitude earthquake in February 2023 devastated southern Turkey and northern Syria, killing over 55,000 people. The ICRC deployed emergency response teams to provide medical care, food, water, shelter, and psychological support to over 1.5 million displaced survivors. Funds support ongoing reconstruction and family tracing services.",
                    StartDate   = new DateOnly(2023, 2, 10),
                    EndDate     = nowDate.AddDays(30),
                    MaxDonation = 500_000m,
                    Location    = "Gaziantep, Turkey & Aleppo, Syria",
                    IsDeleted   = false,
                    CreatedAt   = new DateTimeOffset(2023, 2, 12, 0, 0, 0, TimeSpan.Zero)
                },
                new()
                {
                    ProgrammeId = Guid.CreateVersion7(),
                    NgoId       = redCross.NgoId,
                    CauseId     = causeDisaster.CauseId,
                    Name        = "Morocco Earthquake Emergency Response",
                    ImageUrl    = "/images/programmes/morocco-earthquake-response.jpg",
                    Description = "A devastating 6.8-magnitude earthquake struck the High Atlas Mountains of Morocco in September 2023, killing nearly 3,000 people and injuring thousands more. The ICRC mobilised rapid response teams to deliver search-and-rescue support, emergency healthcare, and essential supplies to remote mountain villages cut off by landslides.",
                    StartDate   = new DateOnly(2023, 9, 9),
                    EndDate     = nowDate.AddDays(60),
                    MaxDonation = 350_000m,
                    Location    = "Marrakesh-Safi Region, Morocco",
                    IsDeleted   = false,
                    CreatedAt   = new DateTimeOffset(2023, 9, 11, 0, 0, 0, TimeSpan.Zero)
                },
                new()
                {
                    ProgrammeId = Guid.CreateVersion7(),
                    NgoId       = unicef.NgoId,
                    CauseId     = causeEducation.CauseId,
                    Name        = "Education Cannot Wait — Ukraine",
                    ImageUrl    = "/images/programmes/ukraine-children-education.jpg",
                    Description = "Since the escalation of the conflict in Ukraine in 2022, over 5 million children have been affected by attacks on education. UNICEF's 'Education Cannot Wait' initiative repairs and equips schools with reinforced shelters, delivers remote learning tools, and provides psychosocial support to keep children learning safely both inside Ukraine and in refugee host countries.",
                    StartDate   = new DateOnly(2022, 3, 1),
                    EndDate     = nowDate.AddDays(180),
                    MaxDonation = 750_000m,
                    Location    = "Kyiv, Kharkiv, Lviv — Ukraine & Poland Border",
                    IsDeleted   = false,
                    CreatedAt   = new DateTimeOffset(2022, 3, 5, 0, 0, 0, TimeSpan.Zero)
                },
                new()
                {
                    ProgrammeId = Guid.CreateVersion7(),
                    NgoId       = saveChildren.NgoId,
                    CauseId     = causeEducation.CauseId,
                    Name        = "Girls' Education in Afghanistan",
                    ImageUrl    = "/images/programmes/afghanistan-girls-education.jpg",
                    Description = "Following the Taliban's ban on girls' secondary education in 2021, Save the Children launched community-based learning centres that provide informal education, psychosocial support, and vocational training to over 200,000 Afghan girls and young women. The programme operates covertly through trusted local networks to ensure participant safety.",
                    StartDate   = new DateOnly(2022, 1, 15),
                    EndDate     = nowDate.AddDays(365),
                    MaxDonation = 600_000m,
                    Location    = "Kabul, Herat & Mazar-i-Sharif, Afghanistan",
                    IsDeleted   = false,
                    CreatedAt   = new DateTimeOffset(2022, 1, 20, 0, 0, 0, TimeSpan.Zero)
                },
                new()
                {
                    ProgrammeId = Guid.CreateVersion7(),
                    NgoId       = msf.NgoId,
                    CauseId     = causeHealth.CauseId,
                    Name        = "Gaza Emergency Medical Aid 2024",
                    ImageUrl    = "/images/programmes/gaza-medical-aid.jpg",
                    Description = "MSF surgical and medical teams are operating in and around Gaza providing emergency trauma care, performing surgeries, treating the wounded, and supporting overwhelmed health facilities. The programme covers emergency obstetric care, mental health support, and supply chains for critical medicines in a besieged environment.",
                    StartDate   = new DateOnly(2023, 10, 10),
                    EndDate     = nowDate.AddDays(90),
                    MaxDonation = 1_000_000m,
                    Location    = "Gaza Strip, Palestinian Territories",
                    IsDeleted   = false,
                    CreatedAt   = new DateTimeOffset(2023, 10, 12, 0, 0, 0, TimeSpan.Zero)
                },
                new()
                {
                    ProgrammeId = Guid.CreateVersion7(),
                    NgoId       = msf.NgoId,
                    CauseId     = causeHealth.CauseId,
                    Name        = "Sudan Cholera & Conflict Response",
                    ImageUrl    = "/images/programmes/sudan-conflict-clinic.jpg",
                    Description = "Sudan's civil war that erupted in April 2023 triggered one of the world's fastest-growing displacement crises. MSF teams are treating mass casualty events, running outpatient therapeutic feeding programmes, fighting a severe cholera outbreak, and providing mental health services. This programme funds field hospitals, water purification, and cholera treatment units.",
                    StartDate   = new DateOnly(2023, 4, 20),
                    EndDate     = nowDate.AddDays(120),
                    MaxDonation = 800_000m,
                    Location    = "Khartoum, Darfur & Port Sudan, Sudan",
                    IsDeleted   = false,
                    CreatedAt   = new DateTimeOffset(2023, 4, 25, 0, 0, 0, TimeSpan.Zero)
                },
                new()
                {
                    ProgrammeId = Guid.CreateVersion7(),
                    NgoId       = wwf.NgoId,
                    CauseId     = causeEnv.CauseId,
                    Name        = "Amazon Rainforest Conservation",
                    ImageUrl    = "/images/programmes/amazon-conservation-patrol.jpg",
                    Description = "The Amazon rainforest — Earth's largest tropical forest — loses an area the size of a football pitch every single minute to deforestation. WWF's Amazon programme works with Indigenous communities to monitor and protect 60 million hectares, prosecute illegal loggers, restore degraded land, and advocate for stronger international legal frameworks.",
                    StartDate   = new DateOnly(2024, 1, 1),
                    EndDate     = new DateOnly(2026, 12, 31),
                    MaxDonation = 2_000_000m,
                    Location    = "Pará & Amazonas States, Brazil",
                    IsDeleted   = false,
                    CreatedAt   = new DateTimeOffset(2024, 1, 3, 0, 0, 0, TimeSpan.Zero)
                },
                new()
                {
                    ProgrammeId = Guid.CreateVersion7(),
                    NgoId       = wwf.NgoId,
                    CauseId     = causeEnv.CauseId,
                    Name        = "Great Barrier Reef Restoration",
                    ImageUrl    = "/images/programmes/great-barrier-reef-restoration.jpg",
                    Description = "The Great Barrier Reef has lost over 50% of its coral cover since 1995 due to climate change-driven bleaching. WWF's reef restoration programme supports coral gardening nurseries, develops heat-resistant coral strains, works with the Australian government to cut land-based pollution, and reduces illegal fishing in the Marine Park.",
                    StartDate   = new DateOnly(2024, 3, 1),
                    EndDate     = new DateOnly(2027, 3, 1),
                    MaxDonation = 1_500_000m,
                    Location    = "Queensland, Australia",
                    IsDeleted   = false,
                    CreatedAt   = new DateTimeOffset(2024, 3, 5, 0, 0, 0, TimeSpan.Zero)
                },
                new()
                {
                    ProgrammeId = Guid.CreateVersion7(),
                    NgoId       = unicef.NgoId,
                    CauseId     = causeHunger.CauseId,
                    Name        = "Horn of Africa Famine Response",
                    ImageUrl    = "/images/programmes/horn-of-africa-famine-relief.jpg",
                    Description = "The worst drought in 40 years has pushed over 22 million people across Ethiopia, Kenya, and Somalia into acute food insecurity. UNICEF's response deploys Ready-to-Use Therapeutic Food (RUTF) to treat severe acute malnutrition, operates mobile health and nutrition clinics, and supports emergency water trucking and WASH services for displaced families.",
                    StartDate   = new DateOnly(2022, 7, 1),
                    EndDate     = nowDate.AddDays(150),
                    MaxDonation = 900_000m,
                    Location    = "Somali, Oromia (Ethiopia), Turkana (Kenya) & South Somalia",
                    IsDeleted   = false,
                    CreatedAt   = new DateTimeOffset(2022, 7, 5, 0, 0, 0, TimeSpan.Zero)
                },
                new()
                {
                    ProgrammeId = Guid.CreateVersion7(),
                    NgoId       = saveChildren.NgoId,
                    CauseId     = causeHunger.CauseId,
                    Name        = "Clean Water for Sub-Saharan Africa",
                    ImageUrl    = "/images/programmes/sub-saharan-clean-water.jpg",
                    Description = "700 million people worldwide lack access to safe drinking water. Save the Children's clean water initiative drills boreholes, installs solar-powered water pumps, builds rainwater harvesting systems, and trains local WASH technicians across 12 countries in Sub-Saharan Africa. Each borehole provides safe water for up to 1,000 people for 20+ years.",
                    StartDate   = new DateOnly(2024, 6, 1),
                    EndDate     = new DateOnly(2027, 6, 1),
                    MaxDonation = 700_000m,
                    Location    = "Ethiopia, Kenya, Nigeria, South Sudan & Mali",
                    IsDeleted   = false,
                    CreatedAt   = new DateTimeOffset(2024, 6, 3, 0, 0, 0, TimeSpan.Zero)
                },
            };

            await context.WelfareProgrammes.AddRangeAsync(programmes);
            await context.SaveChangesAsync();
        }

        // ─────────────────────────────────────────────────────────────────────
        // 6. GALLERY IMAGES
        // ─────────────────────────────────────────────────────────────────────
        if (!await context.GalleryImages.AnyAsync())
        {
            var programmes = await context.WelfareProgrammes.ToListAsync();
            var gallery = new List<GalleryImage>();

            void AddImages(Guid? progId, params (string url, string caption)[] images)
            {
                foreach (var (url, caption) in images)
                    gallery.Add(new GalleryImage { ImageId = Guid.CreateVersion7(), ProgrammeId = progId, ImageUrl = url, Caption = caption });
            }

            var eqk = programmes.FirstOrDefault(p => p.Name.Contains("Turkey"))?.ProgrammeId;
            var ukraine = programmes.FirstOrDefault(p => p.Name.Contains("Ukraine"))?.ProgrammeId;
            var amazon = programmes.FirstOrDefault(p => p.Name.Contains("Amazon"))?.ProgrammeId;
            var gaza = programmes.FirstOrDefault(p => p.Name.Contains("Gaza"))?.ProgrammeId;
            var cleanW = programmes.FirstOrDefault(p => p.Name.Contains("Clean Water"))?.ProgrammeId;

            AddImages(eqk,
                ("/images/gallery/earthquake-rescue-rubble.jpg", "Search and rescue teams clearing rubble"),
                ("/images/gallery/station.jpg", "Temporary shelter camps for 20,000 survivors"),
                ("/images/gallery/earthquake-medical-tent.jpg", "Field medical station treating the injured"));

            AddImages(ukraine,
                ("/images/gallery/ukraine-mobile-classroom.jpg", "Mobile classroom unit deployed to refugee camp"),
                ("/images/gallery/ukraine-bunker-study.jpg", "Children continuing education despite the conflict"));

            AddImages(amazon,
                ("/images/gallery/amazon-indigenous-patrol.jpg", "Indigenous rangers patrolling against illegal logging"),
                ("/images/gallery/amazon-tree-planting.jpg", "Reforestation planting day with 500 volunteers"),
                ("/images/gallery/drone.jpg", "Drone aerial survey mapping deforestation hotspots"));

            AddImages(gaza,
                ("/images/gallery/gaza-field-hospital-surgery.jpg", "MSF surgeons operating in a field hospital"),
                ("/images/gallery/medical-aid-convoy.jpg", "Critical medical supplies arriving by convoy"));

            AddImages(cleanW,
                ("/images/gallery/africa-borehole-drilling.jpg", "Solar-powered borehole serving 1,200 villagers"),
                ("/images/gallery/village-water-pump.jpg", "Community water pump installation complete"));

            // Standalone gallery
            AddImages(null,
                ("/images/gallery/giveaid-gala-dinner.jpg", "GiveAID Annual Fundraising Gala 2025"),
                ("/images/gallery/giveaid-volunteer-group.jpg", "Global Volunteer Day — 3,000 participants"));

            await context.GalleryImages.AddRangeAsync(gallery);
            await context.SaveChangesAsync();
        }

        // ─────────────────────────────────────────────────────────────────────
        // 7. NGO PARTNERS (link NGOs to Corporate Partners)
        // ─────────────────────────────────────────────────────────────────────
        if (!await context.NgoPartners.AnyAsync())
        {
            var ngos = await context.Ngos.ToListAsync();
            var partners = await context.CorporatePartners.ToListAsync();

            var links = new List<NgoPartner>();
            void Link(string ngoName, string partnerName)
            {
                var n = ngos.FirstOrDefault(x => x.Name == ngoName);
                var p = partners.FirstOrDefault(x => x.Name == partnerName);
                if (n != null && p != null)
                    links.Add(new NgoPartner { NgoId = n.NgoId, PartnerId = p.PartnerId });
            }

            Link("UNICEF", "Viettel Group");
            Link("UNICEF", "Vingroup JSC");
            Link("World Wildlife Fund (WWF)", "FPT Corporation");
            Link("World Wildlife Fund (WWF)", "Masan Group");
            Link("Médecins Sans Frontières (MSF)", "Techcombank");
            Link("International Red Cross (ICRC)", "Viettel Group");
            Link("Save the Children", "FPT Corporation");

            if (links.Count > 0)
                await context.NgoPartners.AddRangeAsync(links);

            await context.SaveChangesAsync();
        }

        // ─────────────────────────────────────────────────────────────────────
        // 8. USER INTERESTS
        // ─────────────────────────────────────────────────────────────────────
        if (!await context.UserInterests.AnyAsync())
        {
            var users = await context.Users.Where(u => u.Role == UserRole.Member).ToListAsync();
            var ngos = await context.Ngos.ToListAsync();

            var interests = new List<UserInterest>();
            void Interested(int userIdx, string ngoName)
            {
                if (userIdx >= users.Count) return;
                var n = ngos.FirstOrDefault(x => x.Name == ngoName);
                if (n != null) interests.Add(new UserInterest { UserId = users[userIdx].UserId, NgoId = n.NgoId });
            }

            Interested(0, "International Red Cross (ICRC)");
            Interested(0, "UNICEF");
            Interested(1, "Médecins Sans Frontières (MSF)");
            Interested(2, "World Wildlife Fund (WWF)");
            Interested(2, "International Red Cross (ICRC)");
            Interested(3, "Save the Children");
            Interested(3, "UNICEF");
            Interested(4, "World Wildlife Fund (WWF)");
            Interested(5, "UNICEF");
            Interested(6, "Médecins Sans Frontières (MSF)");
            Interested(7, "Save the Children");
            Interested(8, "International Red Cross (ICRC)");
            Interested(9, "World Wildlife Fund (WWF)");

            if (interests.Count > 0)
                await context.UserInterests.AddRangeAsync(interests);

            await context.SaveChangesAsync();
        }

        // ─────────────────────────────────────────────────────────────────────
        // 9. TRANSACTIONS & DONATIONS  (20 realistic donations)
        // ─────────────────────────────────────────────────────────────────────
        if (!await context.Transactions.AnyAsync())
        {
            var users = await context.Users.Where(u => u.Role == UserRole.Member).ToListAsync();
            var ngos = await context.Ngos.ToListAsync();
            var programmes = await context.WelfareProgrammes.ToListAsync();
            var causes = await context.DonationCauses.ToListAsync();

            var txns = new List<Transaction>();
            var dons = new List<Donation>();
            var rng = new Random(42);

            string[] gateways = ["VNPay", "MoMo", "ZaloPay", "PayPal"];
            string[] accounts = ["9704366800001122", "0901110001", "0934567890", "paypal@donor.com",
                                 "9704366800003344", "0956789012", "9704366800005566", "0978123456",
                                 "zalo@donor.vn",   "9704366800007788"];

            void AddDonation(int userIdx, string ngoName, string programmeName, string causeName,
                             decimal amount, string gateway, string account, string note, string refCode,
                             DateTimeOffset time, DonationStatus status = DonationStatus.Completed)
            {
                if (userIdx >= users.Count) return;
                var user = users[userIdx];
                var ngo = ngos.FirstOrDefault(n => n.Name == ngoName);
                var prog = programmes.FirstOrDefault(p => p.Name.Contains(programmeName));
                var caus = causes.FirstOrDefault(c => c.Name == causeName);
                var txId = Guid.CreateVersion7();
                txns.Add(new Transaction
                {
                    TransactionId = txId,
                    Gateway = gateway,
                    AccountNumber = account,
                    Content = note,
                    Amount = amount,
                    ReferenceCode = refCode,
                    TransactionTime = time
                });
                dons.Add(new Donation
                {
                    DonationId = Guid.CreateVersion7(),
                    UserId = user.UserId,
                    NgoId = ngo?.NgoId,
                    ProgrammeId = prog?.ProgrammeId,
                    CauseId = caus?.CauseId,
                    TransactionId = txId,
                    Amount = amount,
                    Status = status,
                    CreatedAt = time
                });
            }

            // 20 realistic donations spread across donors, programmes, and time
            AddDonation(0, "International Red Cross (ICRC)", "Turkey-Syria", "Disaster Relief", 2500m, "VNPay", accounts[0], "Earthquake relief donation", "TXN-001-ICRC-EQ", now.AddMonths(-5).AddDays(-10));
            AddDonation(1, "Médecins Sans Frontières (MSF)", "Gaza", "Healthcare", 1500m, "MoMo", accounts[1], "Medical aid for Gaza", "TXN-002-MSF-GZ", now.AddMonths(-5).AddDays(-5));
            AddDonation(2, "World Wildlife Fund (WWF)", "Amazon", "Environment", 3000m, "PayPal", accounts[3], "Save the Amazon rainforest", "TXN-003-WWF-AMZ", now.AddMonths(-5).AddDays(1));
            AddDonation(3, "UNICEF", "Ukraine", "Education", 1000m, "ZaloPay", accounts[2], "Education for Ukrainian children", "TXN-004-UNI-UKR", now.AddMonths(-4).AddDays(-12));
            AddDonation(4, "Save the Children", "Clean Water", "Hunger & Food Aid", 750m, "VNPay", accounts[4], "Clean water access Africa", "TXN-005-STC-WAT", now.AddMonths(-4).AddDays(-2));
            AddDonation(5, "International Red Cross (ICRC)", "Morocco", "Disaster Relief", 500m, "MoMo", accounts[5], "Morocco earthquake support", "TXN-006-ICRC-MAR", now.AddMonths(-4).AddDays(3));
            AddDonation(6, "UNICEF", "Horn of Africa", "Hunger & Food Aid", 1200m, "PayPal", accounts[3], "Famine response Horn of Africa", "TXN-007-UNI-HOA", now.AddMonths(-3).AddDays(-8));
            AddDonation(7, "Médecins Sans Frontières (MSF)", "Sudan", "Healthcare", 800m, "VNPay", accounts[7], "Medical aid for Sudan conflict", "TXN-008-MSF-SDN", now.AddMonths(-3).AddDays(2));
            AddDonation(8, "World Wildlife Fund (WWF)", "Great Barrier", "Environment", 2000m, "ZaloPay", accounts[8], "Reef restoration project", "TXN-009-WWF-GBR", now.AddMonths(-2).AddDays(-10));
            AddDonation(9, "Save the Children", "Girls' Education", "Education", 1800m, "MoMo", accounts[9], "Girls education Afghanistan", "TXN-010-STC-AFG", now.AddMonths(-2).AddDays(-3));
            AddDonation(0, "UNICEF", "Ukraine", "Education", 500m, "VNPay", accounts[0], "Ukraine children education fund", "TXN-011-UNI-UKR2", now.AddMonths(-2).AddDays(1));
            AddDonation(1, "International Red Cross (ICRC)", "Turkey-Syria", "Disaster Relief", 3000m, "ZaloPay", accounts[1], "Earthquake survivors support", "TXN-012-ICRC-EQ2", now.AddMonths(-1).AddDays(-15));
            AddDonation(2, "Save the Children", "Clean Water", "Hunger & Food Aid", 600m, "MoMo", accounts[2], "Water for African communities", "TXN-013-STC-WAT2", now.AddMonths(-1).AddDays(-8));
            AddDonation(3, "World Wildlife Fund (WWF)", "Amazon", "Environment", 4500m, "PayPal", accounts[3], "Amazon deforestation protection", "TXN-014-WWF-AMZ2", now.AddMonths(-1).AddDays(2));
            AddDonation(4, "Médecins Sans Frontières (MSF)", "Gaza", "Healthcare", 2200m, "VNPay", accounts[4], "Critical medical supplies Gaza", "TXN-015-MSF-GZ2", now.AddDays(-20));
            AddDonation(5, "UNICEF", "Horn of Africa", "Hunger & Food Aid", 950m, "MoMo", accounts[5], "Emergency nutrition Africa", "TXN-016-UNI-HOA2", now.AddDays(-15));
            AddDonation(6, "International Red Cross (ICRC)", "Morocco", "Disaster Relief", 1100m, "ZaloPay", accounts[6], "Moroccan earthquake relief second fund", "TXN-017-ICRC-MAR2", now.AddDays(-10));
            AddDonation(7, "World Wildlife Fund (WWF)", "Great Barrier", "Environment", 1700m, "VNPay", accounts[7], "Coral reef restoration support", "TXN-018-WWF-GBR2", now.AddDays(-5));
            AddDonation(8, "Save the Children", "Girls' Education", "Education", 850m, "MoMo", accounts[8], "Empowering girls through education", "TXN-019-STC-AFG2", now.AddDays(-2));
            // 1 voided donation for dashboard realism
            AddDonation(9, "Médecins Sans Frontières (MSF)", "Sudan", "Healthcare", 400m, "VNPay", accounts[9], "Sudan medical supplies donation", "TXN-020-MSF-SDN2", now.AddDays(-1), DonationStatus.Void);

            await context.Transactions.AddRangeAsync(txns);
            await context.Donations.AddRangeAsync(dons);
            await context.SaveChangesAsync();
        }

        // ─────────────────────────────────────────────────────────────────────
        // 10. NOTIFICATIONS
        // ─────────────────────────────────────────────────────────────────────
        if (!await context.Notifications.AnyAsync())
        {
            var users = await context.Users.Where(u => u.Role == UserRole.Member).ToListAsync();
            var donations = await context.Donations.Include(d => d.Programme).ToListAsync();

            var notifications = new List<Notification>();
            foreach (var donation in donations.Where(d => d.Status == DonationStatus.Completed).Take(15))
            {
                var user = users.FirstOrDefault(u => u.UserId == donation.UserId);
                if (user == null) continue;
                notifications.Add(new Notification
                {
                    NotificationId = Guid.CreateVersion7(),
                    UserId = user.UserId,
                    Content = $"Your donation of ${donation.Amount:N0} was successfully received. Thank you for your generosity!",
                    IsRead = donation.CreatedAt < DateTimeOffset.UtcNow.AddDays(-14),
                    CreatedAt = donation.CreatedAt.AddSeconds(30)
                });
            }

            if (notifications.Count > 0)
                await context.Notifications.AddRangeAsync(notifications);

            await context.SaveChangesAsync();
        }

        // ─────────────────────────────────────────────────────────────────────
        // 11. USER QUERIES (5 pending, 5 answered)
        // ─────────────────────────────────────────────────────────────────────
        if (!await context.UserQueries.AnyAsync())
        {
            var users = await context.Users.Where(u => u.Role == UserRole.Member).OrderBy(u => u.CreatedAt).ToListAsync();
            var queries = new List<UserQuery>();

            void Query(int idx, string subject, string message, string? reply, int daysAgo)
            {
                if (idx >= users.Count) return;
                queries.Add(new UserQuery
                {
                    QueryId = Guid.CreateVersion7(),
                    UserId = users[idx].UserId,
                    Subject = subject,
                    MessageText = message,
                    ReplyText = reply,
                    CreatedAt = DateTimeOffset.UtcNow.AddDays(-daysAgo)
                });
            }

            // Answered queries
            Query(0, "How can I get a donation receipt?", "I donated to the Turkey-Syria Earthquake Relief fund last month. Could I please receive an official receipt for corporate tax records?", "Thank you for your donation! You can download your official receipt from My Account > Donation History > View > Download Receipt. For corporate receipts, email receipts@giveaid.com with your company details.", 30);
            Query(1, "Is my payment data secure?", "I used VNPay to donate. I want to make sure my card data and transaction information are encrypted and protected.", "Your security is our top priority. GiveAID uses 256-bit TLS encryption and does not store any card data. All payments are processed through PCI-DSS-compliant gateways. You are completely safe.", 25);
            Query(2, "Can I sponsor a specific programme?", "We are a Vietnamese SME interested in becoming a corporate sponsor specifically for the Amazon Conservation programme. What are the tiers?", "Thank you for your interest! Corporate sponsorship starts at $5,000 with logo placement, reporting, and co-branding options. Please email partnerships@giveaid.com and our team will send a full prospectus.", 20);
            Query(3, "How are donations distributed to NGOs?", "I want to understand your fee structure. What percentage of my donation actually reaches the NGO vs administrative costs?", "GiveAID operates with a 3% platform fee on transactions. 97% of every donation goes directly to the designated NGO programme. All financials are published in our Annual Transparency Report on our website.", 18);
            Query(4, "Donation not reflecting in my history", "I made a $750 donation via ZaloPay 3 days ago but it does not show in my donation history. The money has been deducted from my account.", "We apologise for the delay! Our team identified a ZaloPay webhook issue that has now been resolved. Your donation has been manually confirmed and is now visible in your history. Thank you for your patience.", 12);
            // Pending queries
            Query(5, "Can I set up a recurring monthly donation?", "Is there a way to schedule an automatic monthly donation to the Girls' Education in Afghanistan programme? I want to commit to $100/month.", null, 7);
            Query(6, "Volunteering opportunities with MSF", "I am a licensed physician based in Ho Chi Minh City and I am very interested in volunteering with Médecins Sans Frontières. How do I apply through GiveAID?", null, 5);
            Query(7, "Difference between Cause and Programme?", "I am confused about the difference between donating to a 'Cause' vs a specific 'Welfare Programme'. Could you explain which is better for tracking impact?", null, 3);
            Query(8, "Requesting a refund on voided donation", "My donation of $400 to MSF Sudan was marked as Voided but I can see the charge on my VNPay account statement. Please assist with the refund urgently.", null, 2);
            Query(9, "Partnership for university CSR project", "I am a final-year student working on a CSR project. Can I partner with GiveAID to run a fundraising campaign for our university? What is the process?", null, 1);

            await context.UserQueries.AddRangeAsync(queries);
            await context.SaveChangesAsync();
        }

        // ─────────────────────────────────────────────────────────────────────
        // 12. ABOUT US SUBPAGES
        // ─────────────────────────────────────────────────────────────────────
        if (!await context.AboutUsSubpages.AnyAsync())
        {
            var admin = await context.Users.FirstOrDefaultAsync(u => u.Role == UserRole.Admin);

            var subpages = new List<AboutUsSubpage>
            {
                new() { SubpageId = Guid.CreateVersion7(), Slug = "our-mission", Title = "Our Mission" },
                new() { SubpageId = Guid.CreateVersion7(), Slug = "our-story",   Title = "Our Story"   },
                new() { SubpageId = Guid.CreateVersion7(), Slug = "our-team",    Title = "Our Team"    },
            };

            await context.AboutUsSubpages.AddRangeAsync(subpages);
            await context.SaveChangesAsync();

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
                        "<p>At GiveAID, our mission is to connect compassionate donors with life-changing welfare programmes run by the world's most trusted humanitarian organisations — including UNICEF, the Red Cross, WWF, MSF, and Save the Children. We believe every dollar donated with purpose has the power to change a life.</p>" +
                        "<h2>Our Commitment</h2>" +
                        "<p>We operate with radical transparency. 97% of every donation reaches the designated programme. All financials are independently audited and published in our annual report. Donors receive real-time impact updates directly from field teams.</p>" +
                        "</section>",
                    CreatedAt = DateTimeOffset.UtcNow.AddDays(-180)
                },
                new()
                {
                    ModificationId = Guid.CreateVersion7(),
                    SubpageId      = subpages[1].SubpageId,
                    UserId         = admin?.UserId,
                    HtmlContent    =
                        "<section class=\"story-section\">" +
                        "<h2>How It All Started</h2>" +
                        "<p>GiveAID was founded in 2018 by a group of international development professionals and technologists who witnessed first-hand the friction between well-meaning donors and the NGOs that desperately needed their support. The idea was simple: build trust through technology.</p>" +
                        "<h2>Where We Are Today</h2>" +
                        "<p>Today, GiveAID partners with 5 globally recognised NGOs operating in 190+ countries. We have facilitated over $12 million in donations, funded 48 welfare programmes, and directly impacted 2.3 million beneficiaries. We are just getting started.</p>" +
                        "</section>",
                    CreatedAt = DateTimeOffset.UtcNow.AddDays(-180)
                },
                new()
                {
                    ModificationId = Guid.CreateVersion7(),
                    SubpageId      = subpages[2].SubpageId,
                    UserId         = admin?.UserId,
                    HtmlContent    =
                        "<section class=\"team-section\">" +
                        "<h2>The People Behind GiveAID</h2>" +
                        "<p>Our lean, diverse team spans Vietnam, Switzerland, and the United States. We bring together expertise in humanitarian finance, full-stack engineering, UX design, and international development. Every team member has personally volunteered in at least one of our partner NGO programmes.</p>" +
                        "<h2>Join the Movement</h2>" +
                        "<p>We are always seeking passionate volunteers, corporate partners, and impact investors. If you share our belief in a more generous world, <a href=\"/contact\">reach out to us</a> — we would love to hear from you.</p>" +
                        "</section>",
                    CreatedAt = DateTimeOffset.UtcNow.AddDays(-180)
                },
            };

            await context.UserModifications.AddRangeAsync(modifications);
            await context.SaveChangesAsync();
        }
    }
}
