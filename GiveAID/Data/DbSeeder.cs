using GiveAID.Models;
using GiveAID.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context, IPasswordService passwordService)
    {
        var random = new Random(42);

        // 1. Seed Users
        if (!await context.Users.AnyAsync())
        {
            var users = new List<User>
            {
                new() { FullName = "Admin User", Email = "admin@giveaid.com", PasswordHash = passwordService.HashPassword("Admin@123"), DateOfBirth = new DateOnly(1980, 1, 1), Address = "123 Admin St", PhoneNumber = "+14155550001", Occupation = "Administrator", Role = UserRole.Admin, IsDeleted = false },
                new() { FullName = "John Doe", Email = "john@example.com", PasswordHash = passwordService.HashPassword("Member@123"), DateOfBirth = new DateOnly(1990, 5, 15), Address = "456 Member Ave", PhoneNumber = "+442079460002", Occupation = "Software Engineer", Role = UserRole.Member, IsDeleted = false },
                new() { FullName = "Jane Smith", Email = "jane@example.com", PasswordHash = passwordService.HashPassword("Member@123"), DateOfBirth = new DateOnly(1995, 8, 20), Address = "789 User Blvd", PhoneNumber = "+61491570156", Occupation = "Teacher", Role = UserRole.Member, IsDeleted = false },
                new() { FullName = "Michael Tran", Email = "michael@example.com", PasswordHash = passwordService.HashPassword("Member@123"), DateOfBirth = new DateOnly(1988, 3, 10), Address = "321 Oak Road", PhoneNumber = "+819012345678", Occupation = "Doctor", Role = UserRole.Member, IsDeleted = false },
                new() { FullName = "Emily Nguyen", Email = "emily@example.com", PasswordHash = passwordService.HashPassword("Member@123"), DateOfBirth = new DateOnly(1993, 11, 25), Address = "654 Pine St", PhoneNumber = "+4915123456789", Occupation = "Designer", Role = UserRole.Member, IsDeleted = false },
                new() { FullName = "David Clark", Email = "david@example.com", PasswordHash = passwordService.HashPassword("Member@123"), DateOfBirth = new DateOnly(1985, 2, 14), Address = "111 Maple St", PhoneNumber = "+33612345678", Occupation = "Architect", Role = UserRole.Member, IsDeleted = false },
                new() { FullName = "Sophia Martinez", Email = "sophia@example.com", PasswordHash = passwordService.HashPassword("Member@123"), DateOfBirth = new DateOnly(1992, 7, 8), Address = "222 Birch St", PhoneNumber = "+84912345678", Occupation = "Nurse", Role = UserRole.Member, IsDeleted = false },
                new() { FullName = "James Taylor", Email = "james@example.com", PasswordHash = passwordService.HashPassword("Member@123"), DateOfBirth = new DateOnly(1980, 10, 30), Address = "333 Cedar St", PhoneNumber = "+5511999999999", Occupation = "Lawyer", Role = UserRole.Member, IsDeleted = false },
                new() { FullName = "Olivia Anderson", Email = "olivia@example.com", PasswordHash = passwordService.HashPassword("Member@123"), DateOfBirth = new DateOnly(1998, 12, 12), Address = "444 Elm St", PhoneNumber = "+919876543210", Occupation = "Student", Role = UserRole.Member, IsDeleted = false },
                new() { FullName = "Robert Thomas", Email = "robert@example.com", PasswordHash = passwordService.HashPassword("Member@123"), DateOfBirth = new DateOnly(1975, 4, 22), Address = "555 Spruce St", PhoneNumber = "+27821234567", Occupation = "Manager", Role = UserRole.Member, IsDeleted = false },
                new() { FullName = "Emma Jackson", Email = "emma@example.com", PasswordHash = passwordService.HashPassword("Member@123"), DateOfBirth = new DateOnly(1991, 6, 18), Address = "666 Ash St", PhoneNumber = "+525512345678", Occupation = "Writer", Role = UserRole.Member, IsDeleted = false },
                new() { FullName = "William White", Email = "william@example.com", PasswordHash = passwordService.HashPassword("Member@123"), DateOfBirth = new DateOnly(1987, 9, 5), Address = "777 Fir St", PhoneNumber = "+79161234567", Occupation = "Engineer", Role = UserRole.Member, IsDeleted = false },
                new() { FullName = "Mia Harris", Email = "mia@example.com", PasswordHash = passwordService.HashPassword("Member@123"), DateOfBirth = new DateOnly(1996, 3, 27), Address = "888 Willow St", PhoneNumber = "+34612345678", Occupation = "Artist", Role = UserRole.Member, IsDeleted = false },
                new() { FullName = "Alexander Martin", Email = "alexander@example.com", PasswordHash = passwordService.HashPassword("Member@123"), DateOfBirth = new DateOnly(1982, 11, 11), Address = "999 Poplar St", PhoneNumber = "+393123456789", Occupation = "Chef", Role = UserRole.Member, IsDeleted = false },
                new() { FullName = "Charlotte Thompson", Email = "charlotte@example.com", PasswordHash = passwordService.HashPassword("Member@123"), DateOfBirth = new DateOnly(1994, 1, 20), Address = "000 Redwood St", PhoneNumber = "+8613812345678", Occupation = "Pharmacist", Role = UserRole.Member, IsDeleted = false },
                new() { FullName = "Harper Moore", Email = "harper@example.com", PasswordHash = passwordService.HashPassword("Member@123"), DateOfBirth = new DateOnly(1989, 8, 14), Address = "101 Ocean Ave", PhoneNumber = "+821012345678", Occupation = "Journalist", Role = UserRole.Member, IsDeleted = false }
            };
            await context.Users.AddRangeAsync(users);
        }

        // 2. Seed Donation Causes
        if (!await context.DonationCauses.AnyAsync())
        {
            var causes = new List<DonationCause>
            {
                new() { Name = "Education" },
                new() { Name = "Healthcare" },
                new() { Name = "Environment" },
                new() { Name = "Disaster Relief" },
                new() { Name = "Animal Welfare" },
                new() { Name = "Poverty Alleviation" },
                new() { Name = "Clean Water" },
                new() { Name = "Human Rights" },
                new() { Name = "Arts and Culture" },
                new() { Name = "Mental Health" },
                new() { Name = "Elderly Care" },
                new() { Name = "Disability Support" },
                new() { Name = "Hunger Relief" },
                new() { Name = "Youth Development" },
                new() { Name = "Women Empowerment" },
                new() { Name = "Community Building" }
            };
            await context.DonationCauses.AddRangeAsync(causes);
        }

        await context.SaveChangesAsync();

        // 3. Seed NGOs
        if (!await context.Ngos.AnyAsync())
        {
            var ngos = new List<Ngo>
            {
                new() { Name = "Save The Children", Description = "Working to improve the lives of children.", Address = "100 Charity Lane", PhoneNumber = "+12125551234", Website = "https://savethechildren.org" },
                new() { Name = "Green Earth Foundation", Description = "Dedicated to environmental conservation.", Address = "200 Green St", PhoneNumber = "+441314960123", Website = "https://greenearth.org" },
                new() { Name = "Health First Aid", Description = "Providing medical assistance.", Address = "300 Medical Ave", PhoneNumber = "+61298765432", Website = "https://healthfirst.org" },
                new() { Name = "Water for Life", Description = "Ensuring clean water access.", Address = "400 Water Way", PhoneNumber = "+81312345678", Website = "https://waterforlife.org" },
                new() { Name = "Habitat Builders", Description = "Building homes for the homeless.", Address = "500 Shelter St", PhoneNumber = "+4930123456", Website = "https://habitatbuilders.org" },
                new() { Name = "Mindful Matters", Description = "Mental health support and awareness.", Address = "600 Peace Blvd", PhoneNumber = "+33123456789", Website = "https://mindfulmatters.org" },
                new() { Name = "Golden Years Charity", Description = "Supporting the elderly.", Address = "700 Wisdom Rd", PhoneNumber = "+84243123456", Website = "https://goldenyears.org" },
                new() { Name = "Equality Now", Description = "Fighting for human rights.", Address = "800 Justice Ave", PhoneNumber = "+552199999999", Website = "https://equalitynow.org" },
                new() { Name = "Global Food Bank", Description = "Eradicating hunger globally.", Address = "900 Nourish St", PhoneNumber = "+911123456789", Website = "https://globalfoodbank.org" },
                new() { Name = "Youth Future Trust", Description = "Empowering the next generation.", Address = "101 Youth Ln", PhoneNumber = "+27211234567", Website = "https://youthfuture.org" },
                new() { Name = "Women Forward", Description = "Promoting women's rights.", Address = "202 Equality St", PhoneNumber = "+528112345678", Website = "https://womenforward.org" },
                new() { Name = "Arts Reach", Description = "Bringing arts to communities.", Address = "303 Culture Blvd", PhoneNumber = "+74951234567", Website = "https://artsreach.org" },
                new() { Name = "Human Rights Watchers", Description = "Monitoring and protecting human rights.", Address = "404 Watcher Rd", PhoneNumber = "+34911234567", Website = "https://hrwatchers.org" },
                new() { Name = "Disability Alliance", Description = "Advocating for disability rights.", Address = "505 Inclusion Ave", PhoneNumber = "+390612345678", Website = "https://disabilityalliance.org" },
                new() { Name = "Paws and Claws Rescue", Description = "Rescuing and rehabilitating animals.", Address = "606 Rescue Ln", PhoneNumber = "+861012345678", Website = "https://pawsandclaws.org" },
                new() { Name = "Community Heroes", Description = "Local community improvement.", Address = "707 Hero Blvd", PhoneNumber = "+82212345678", Website = "https://communityheroes.org" }
            };
            await context.Ngos.AddRangeAsync(ngos);
        }

        await context.SaveChangesAsync();

        // 4. Seed Corporate Partners
        if (!await context.CorporatePartners.AnyAsync())
        {
            var partners = new List<CorporatePartner>
            {
                new() { Name = "Apple", LogoUrl = "https://logo.clearbit.com/apple.com", WebsiteLink = "https://apple.com" },
                new() { Name = "Microsoft", LogoUrl = "https://logo.clearbit.com/microsoft.com", WebsiteLink = "https://microsoft.com" },
                new() { Name = "Google", LogoUrl = "https://logo.clearbit.com/google.com", WebsiteLink = "https://google.com" },
                new() { Name = "Amazon", LogoUrl = "https://logo.clearbit.com/amazon.com", WebsiteLink = "https://amazon.com" },
                new() { Name = "Meta", LogoUrl = "https://logo.clearbit.com/meta.com", WebsiteLink = "https://meta.com" },
                new() { Name = "Netflix", LogoUrl = "https://logo.clearbit.com/netflix.com", WebsiteLink = "https://netflix.com" },
                new() { Name = "Tesla", LogoUrl = "https://logo.clearbit.com/tesla.com", WebsiteLink = "https://tesla.com" },
                new() { Name = "Intel", LogoUrl = "https://logo.clearbit.com/intel.com", WebsiteLink = "https://intel.com" },
                new() { Name = "IBM", LogoUrl = "https://logo.clearbit.com/ibm.com", WebsiteLink = "https://ibm.com" },
                new() { Name = "Oracle", LogoUrl = "https://logo.clearbit.com/oracle.com", WebsiteLink = "https://oracle.com" },
                new() { Name = "Cisco", LogoUrl = "https://logo.clearbit.com/cisco.com", WebsiteLink = "https://cisco.com" },
                new() { Name = "Adobe", LogoUrl = "https://logo.clearbit.com/adobe.com", WebsiteLink = "https://adobe.com" },
                new() { Name = "Salesforce", LogoUrl = "https://logo.clearbit.com/salesforce.com", WebsiteLink = "https://salesforce.com" },
                new() { Name = "Nvidia", LogoUrl = "https://logo.clearbit.com/nvidia.com", WebsiteLink = "https://nvidia.com" },
                new() { Name = "Samsung", LogoUrl = "https://logo.clearbit.com/samsung.com", WebsiteLink = "https://samsung.com" },
                new() { Name = "Sony", LogoUrl = "https://logo.clearbit.com/sony.com", WebsiteLink = "https://sony.com" }
            };
            await context.CorporatePartners.AddRangeAsync(partners);
        }

        await context.SaveChangesAsync();

        var dbCauses = await context.DonationCauses.ToListAsync();
        var dbNgos = await context.Ngos.ToListAsync();
        var dbUsers = await context.Users.ToListAsync();
        var dbPartners = await context.CorporatePartners.ToListAsync();

        // 5. Seed Welfare Programmes
        if (!await context.WelfareProgrammes.AnyAsync())
        {
            var images = new[]
            {
                "https://images.unsplash.com/photo-1532629345422-7515f3d16bb0?w=800&q=80",
                "https://images.unsplash.com/photo-1488521787991-ed7bbaae773c?w=800&q=80",
                "https://images.unsplash.com/photo-1469571486292-0ba58a3f068b?w=800&q=80",
                "https://images.unsplash.com/photo-1593113565632-e4367f0874e0?w=800&q=80",
                "https://images.unsplash.com/photo-1518398092300-5cca33fce812?w=800&q=80",
                "https://images.unsplash.com/photo-1542601906990-b4d3fb778e09?w=800&q=80",
                "https://images.unsplash.com/photo-1483058712412-422a5eee6ce7?w=800&q=80",
                "https://images.unsplash.com/photo-1509099836639-ebd73bd3a655?w=800&q=80",
                "https://images.unsplash.com/photo-1578357061584-811c7fa154bb?w=800&q=80",
                "https://images.unsplash.com/photo-1584043720379-b56cd91f1242?w=800&q=80",
                "https://images.unsplash.com/photo-1473649085228-583485e6e4d7?w=800&q=80",
                "https://images.unsplash.com/photo-1497435334941-8c899ea9b400?w=800&q=80",
                "https://images.unsplash.com/photo-1526951521990-620dc14c214b?w=800&q=80",
                "https://images.unsplash.com/photo-1500382017468-9049fed747ef?w=800&q=80",
                "https://images.unsplash.com/photo-1460518451285-9a4cb39ac9a8?w=800&q=80",
                "https://images.unsplash.com/photo-1593113584824-c8bdce0fa187?w=800&q=80",
                "https://images.unsplash.com/photo-1502224562085-639556652f33?w=800&q=80"
            };

            var programmeData = new (string Name, string Description, string Location, decimal MaxDonation)[]{
                ("Rural Schools for Every Child",        "Building permanent schools in underserved rural communities, providing safe classrooms, trained teachers, and learning materials for thousands of children.",                  "Mekong Delta, Vietnam",          50_000m),
                ("Mobile Health Clinics for the Remote", "Deploying fully equipped mobile medical units to hard-to-reach villages for free checkups, vaccinations, and essential treatments.",                                           "Northern Highland Provinces",    80_000m),
                ("City Reforestation Initiative",        "Planting 10,000 native trees across urban parks and road corridors to reduce pollution, restore biodiversity, and cool our city streets.",                                     "Ho Chi Minh City, Vietnam",      20_000m),
                ("Flood Relief Emergency Fund",          "Emergency shelter kits, clean water tablets, and food parcels for families displaced by devastating seasonal flooding in low-lying coastal areas.",                          "Mekong River Delta",             75_000m),
                ("Digital Literacy for Youth",           "Equipping underserved secondary students with laptops, internet access, and coding skills to bridge the digital divide and open career pathways.",                            "Hanoi, Vietnam",                 30_000m),
                ("Clean Water Wells Project",            "Drilling and maintaining borehole wells to bring safe, clean drinking water to remote villages that currently rely on polluted rivers and ponds.",                            "Central Highlands, Vietnam",     45_000m),
                ("Senior Care & Companionship",          "Providing daily meal deliveries, medical check-ins, and social activities for isolated elderly residents living alone without family support.",                                  "Da Nang, Vietnam",               15_000m),
                ("Animal Rescue & Rehabilitation",       "Rescuing stray and abused animals, providing veterinary care, rehabilitation, and responsible rehoming through our dedicated shelter network.",                                "Hue City, Vietnam",              25_000m),
                ("Women's Entrepreneurship Fund",        "Micro-grants, business training, and mentorship for women in low-income communities to launch sustainable small businesses and achieve financial independence.",              "Can Tho, Vietnam",               60_000m),
                ("Mental Health Awareness Campaign",     "Community workshops, free counselling sessions, and an anti-stigma media campaign to improve mental health awareness and access to support services.",                         "Nationwide, Vietnam",            18_000m),
                ("Disability Inclusion Programme",       "Providing assistive devices, accessible vocational training, and workplace placement support to empower people with disabilities to enter the workforce.",                    "Hai Phong, Vietnam",             35_000m),
                ("Zero Hunger Nutrition Drive",          "Distributing nutritious meal packs and teaching sustainable home gardening to food-insecure families in impoverished urban neighbourhoods.",                                  "Binh Duong Province, Vietnam",   22_000m),
                ("Youth Leadership Academy",             "A six-month leadership and civic engagement programme for promising young people aged 16–24, equipping them with skills to drive positive community change.",                "Nha Trang, Vietnam",             40_000m),
                ("Human Rights Legal Aid Clinic",        "Free legal representation and rights education for marginalised communities facing land disputes, labour exploitation, and domestic abuse.",                                   "Hanoi, Vietnam",                 28_000m),
                ("Ocean Plastic Clean-Up Drive",         "Organising coastal clean-up events, deploying floating barriers in waterways, and funding local recycling cooperatives to combat marine plastic pollution.",                  "Da Nang Coastline, Vietnam",     32_000m),
                ("Arts for All Community Studio",        "Opening free art studios, music rooms, and theatre spaces in underserved neighbourhoods to nurture creativity and provide positive outlets for young people.",              "Hoi An, Vietnam",                12_000m),
                ("Earthquake Preparedness Training",     "Community-level first aid training, emergency drill programmes, and the distribution of disaster preparedness kits to households in seismic-risk zones.",                  "Central Vietnam",                20_000m),
                ("Inclusive Education Support",          "Training teachers in special needs pedagogy and providing adapted learning tools so children with disabilities can thrive in mainstream school environments.",               "Quang Nam Province, Vietnam",    38_000m),
                ("Poverty-Breaking Microloans",          "Providing zero-interest microloans and financial literacy coaching to families trapped in cycles of poverty, enabling them to invest in income-generating activities.",     "Mekong Delta, Vietnam",          55_000m),
                ("Reforestation in the Central Highlands","Restoring degraded forest land in the Central Highlands through large-scale native tree planting, protecting watersheds and the livelihoods of local ethnic communities.", "Gia Lai Province, Vietnam",      48_000m)
            };

            for (int i = 0; i < programmeData.Length; i++)
            {
                var (name, desc, loc, maxDon) = programmeData[i];
                await context.WelfareProgrammes.AddAsync(new WelfareProgramme
                {
                    NgoId       = dbNgos[i % dbNgos.Count].NgoId,
                    CauseId     = dbCauses[i % dbCauses.Count].CauseId,
                    Name        = name,
                    ImageUrl    = images[i % images.Length],
                    Description = desc,
                    StartTime   = DateTimeOffset.UtcNow.AddDays(-random.Next(1, 60)),
                    EndTime     = DateTimeOffset.UtcNow.AddDays(random.Next(30, 365)),
                    MaxDonation = maxDon,
                    Location    = loc
                });
            }
            await context.SaveChangesAsync();
        }

        var dbProgrammes = await context.WelfareProgrammes.ToListAsync();

        // 6. Seed Gallery Images
        if (!await context.GalleryImages.AnyAsync())
        {
            var images = new[]
            {
                "https://images.unsplash.com/photo-1532629345422-7515f3d16bb0?w=800&q=80",
                "https://images.unsplash.com/photo-1488521787991-ed7bbaae773c?w=800&q=80",
                "https://images.unsplash.com/photo-1469571486292-0ba58a3f068b?w=800&q=80",
                "https://images.unsplash.com/photo-1593113565632-e4367f0874e0?w=800&q=80",
                "https://images.unsplash.com/photo-1518398092300-5cca33fce812?w=800&q=80",
                "https://images.unsplash.com/photo-1542601906990-b4d3fb778e09?w=800&q=80",
                "https://images.unsplash.com/photo-1483058712412-422a5eee6ce7?w=800&q=80",
                "https://images.unsplash.com/photo-1509099836639-ebd73bd3a655?w=800&q=80",
                "https://images.unsplash.com/photo-1578357061584-811c7fa154bb?w=800&q=80",
                "https://images.unsplash.com/photo-1584043720379-b56cd91f1242?w=800&q=80",
                "https://images.unsplash.com/photo-1473649085228-583485e6e4d7?w=800&q=80",
                "https://images.unsplash.com/photo-1497435334941-8c899ea9b400?w=800&q=80",
                "https://images.unsplash.com/photo-1526951521990-620dc14c214b?w=800&q=80",
                "https://images.unsplash.com/photo-1500382017468-9049fed747ef?w=800&q=80",
                "https://images.unsplash.com/photo-1460518451285-9a4cb39ac9a8?w=800&q=80",
                "https://images.unsplash.com/photo-1593113584824-c8bdce0fa187?w=800&q=80",
                "https://images.unsplash.com/photo-1502224562085-639556652f33?w=800&q=80"
            };

            var galleryCaptions = new[]
            {
                "Children gathering eagerly on the first day of school",
                "Volunteers laying foundations for a new classroom block",
                "A mobile clinic nurse vaccinating infants in a remote village",
                "Sapling trees being planted along a city boulevard",
                "Flood survivors receiving emergency food and clean water kits",
                "Students participating in an after-school coding workshop",
                "Community members celebrating a newly drilled water well",
                "Elderly residents enjoying a group lunch and music session",
                "Rescued dogs receiving veterinary care at the shelter",
                "Women entrepreneurs showcasing products at a local market",
                "A mental health awareness workshop in a community centre",
                "Workers installing wheelchair ramps at a local school",
                "Families receiving nutritious meal packs at a distribution event",
                "Young leaders presenting their community projects to mentors",
                "Lawyers providing free legal advice at an open clinic",
                "Volunteers collecting plastic waste along a coastal beach",
                "Children painting murals in a new community art studio",
                "Residents practising first aid during an earthquake drill",
                "A teacher using adapted learning tools with a student",
                "A micro-loan recipient opening her first small shop"
            };

            for (int i = 0; i < galleryCaptions.Length; i++)
            {
                await context.GalleryImages.AddAsync(new GalleryImage
                {
                    ImageId     = Guid.CreateVersion7(),
                    ProgrammeId = dbProgrammes[i % dbProgrammes.Count].ProgrammeId,
                    ImageUrl    = images[i % images.Length],
                    Caption     = galleryCaptions[i]
                });
            }
            await context.SaveChangesAsync();
        }

        // 7. Seed NGO Partners
        if (!await context.NgoPartners.AnyAsync())
        {
            for (int i = 0; i < Math.Min(dbNgos.Count, dbPartners.Count); i++)
            {
                var ngoId = dbNgos[i % dbNgos.Count].NgoId;
                var partnerId = dbPartners[(i + 1) % dbPartners.Count].PartnerId;
                
                await context.NgoPartners.AddAsync(new NgoPartner { NgoId = ngoId, PartnerId = partnerId });
            }
            await context.SaveChangesAsync();
        }

        // 8. Seed User Interests
        if (!await context.UserInterests.AnyAsync())
        {
            for (int i = 0; i < Math.Min(dbUsers.Count, dbNgos.Count); i++)
            {
                var userId = dbUsers[i % dbUsers.Count].UserId;
                var ngoId = dbNgos[(i + 2) % dbNgos.Count].NgoId;

                if (!await context.UserInterests.AnyAsync(ui => ui.UserId == userId && ui.NgoId == ngoId))
                {
                    await context.UserInterests.AddAsync(new UserInterest { UserId = userId, NgoId = ngoId });
                }
            }
            await context.SaveChangesAsync();
        }

        // 9. Seed Transactions & Donations
        if (!await context.Transactions.AnyAsync())
        {
            for (int i = 0; i < Math.Min(dbUsers.Count, dbProgrammes.Count); i++)
            {
                var user = dbUsers[i % dbUsers.Count];
                var programme = dbProgrammes[i % dbProgrammes.Count];
                var time = DateTimeOffset.UtcNow.AddDays(-random.Next(1, 60));
                var txId = Guid.CreateVersion7();

                await context.Transactions.AddAsync(new Transaction
                {
                    TransactionId = txId,
                    Gateway = i % 2 == 0 ? "VNPay" : "MoMo",
                    AccountNumber = "1234567890",
                    Content = $"Donation {i + 1}",
                    Amount = random.Next(1, 100) * 10,
                    ReferenceCode = $"TXN-{i + 1:D3}",
                    TransactionTime = time
                });

                await context.Donations.AddAsync(new Donation
                {
                    DonationId = Guid.CreateVersion7(),
                    UserId = user.UserId,
                    ProgrammeId = programme.ProgrammeId,
                    TransactionId = txId,
                    Amount = random.Next(1, 100) * 10,
                    Status = DonationStatus.Completed,
                    CreatedAt = time
                });
            }
            await context.SaveChangesAsync();
        }

        // 10. Seed Notifications
        if (!await context.Notifications.AnyAsync())
        {
            var notificationContents = new[]
            {
                "Your donation of $500 to 'Rural Schools for Every Child' was received. Thank you for making a difference!",
                "New programme 'Flood Relief Emergency Fund' has just launched. Consider lending your support today.",
                "Your donation of $300 to 'City Reforestation Initiative' has been confirmed. 🌳",
                "A welfare programme you follow, 'Digital Literacy for Youth', is now 75% funded. Help push it over the line!",
                "Your donation of $750 was successfully processed via VNPay.",
                "'Clean Water Wells Project' has reached its halfway funding milestone thanks to donors like you!",
                "Thank you for your generous $2,000 donation to 'Mobile Health Clinics for the Remote'.",
                "Reminder: The 'Women's Entrepreneurship Fund' campaign ends in 7 days. Don't miss out!",
                "Your account details have been updated successfully.",
                "A programme you recently donated to has shared a new impact report. Click to read.",
                "Your donation of $400 to 'Rural Schools for Every Child' was received. Thank you!",
                "New NGO 'Community Heroes' is now accepting donations. Check out their programmes.",
                "Your query #5 has received a reply from our support team.",
                "'Ocean Plastic Clean-Up Drive' needs just $3,000 more to reach its goal. Every dollar counts!",
                "Payment method updated: your VNPay card ending in 9012 is now your default.",
                "Thank you! Your $800 donation to 'Arts for All Community Studio' has been confirmed.",
                "Programme update: 'Mobile Health Clinics for the Remote' has treated over 500 patients this month.",
                "Your donation receipt is ready to download from My Donations > View > Download Receipt.",
                "'Youth Leadership Academy' applications are now open. Share with a young person you know!",
                "Your donation of $250 to 'Flood Relief Emergency Fund' is helping displaced families rebuild."
            };

            for (int i = 0; i < notificationContents.Length; i++)
            {
                await context.Notifications.AddAsync(new Notification
                {
                    NotificationId = Guid.CreateVersion7(),
                    UserId         = dbUsers[i % dbUsers.Count].UserId,
                    Content        = notificationContents[i],
                    IsRead         = i % 3 == 0,
                    CreatedAt      = DateTimeOffset.UtcNow.AddDays(-random.Next(1, 30))
                });
            }
            await context.SaveChangesAsync();
        }

        // 11. Seed User Queries
        if (!await context.UserQueries.AnyAsync())
        {
            var john    = dbUsers.FirstOrDefault(u => u.Email == "john@example.com");
            var jane    = dbUsers.FirstOrDefault(u => u.Email == "jane@example.com");
            var michael = dbUsers.FirstOrDefault(u => u.Email == "michael@example.com");

            var queries = new List<UserQuery>();

            // Restored old queries
            if (john != null)
            {
                queries.AddRange(
                [
                    new UserQuery { UserId = john.UserId, Subject = "How do I get a donation receipt?", MessageText = "Hi, I made a donation last week to the school building project. Can I get an official receipt for tax purposes?", ReplyText = "Hi John! Thank you for your generous donation. You can download your receipt from My Donations > View > Download Receipt. Let us know if you need further assistance!", CreatedAt = DateTimeOffset.UtcNow.AddDays(-40) },
                    new UserQuery { UserId = john.UserId, Subject = "Can I set up recurring donations?", MessageText = "Is there a way to set up a monthly recurring donation to a welfare programme?", ReplyText = null, CreatedAt = DateTimeOffset.UtcNow.AddDays(-5) }
                ]);
            }
            if (jane != null)
            {
                queries.Add(new UserQuery { UserId = jane.UserId, Subject = "Voided donation - need help", MessageText = "My donation of $200 was voided. I was charged but the donation did not go through. Please help me resolve this.", ReplyText = "Hi Jane! We sincerely apologize for the inconvenience. Our team has investigated and found a temporary payment gateway issue. A full refund has been initiated and will reflect in 3-5 business days.", CreatedAt = DateTimeOffset.UtcNow.AddDays(-14) });
            }
            if (michael != null)
            {
                queries.Add(new UserQuery { UserId = michael.UserId, Subject = "Partnership opportunities", MessageText = "I work for a hospital and we're interested in partnering with Health First Aid. Who should I contact?", ReplyText = "Hi Michael! We love hearing from healthcare professionals. Please email partnerships@giveaid.com with your organization details and our team will reach out within 48 hours.", CreatedAt = DateTimeOffset.UtcNow.AddDays(-18) });
            }

            // Additional queries to reach 15+
            var additionalQueries = new (string Subject, string Message, string? Reply)[]
            {
                (
                    "How do I update my email address?",
                    "Hello, I recently changed my personal email and would like to update the address on my GiveAID account. Could you guide me through the steps?",
                    "Hi! To update your email, go to My Account > Profile Settings > Edit Email. Enter your new address and confirm with your current password. A verification link will be sent to the new address."
                ),
                (
                    "How can I volunteer for an upcoming event?",
                    "I am very passionate about the cause and would love to volunteer at one of your upcoming charity events. Where do I sign up?",
                    null
                ),
                (
                    "Is my donation tax-deductible?",
                    "I made a donation of $1,000 last month and would like to know if I can claim it as a tax deduction in my country of residence.",
                    "Great question! Tax deductibility depends on your country's laws. In most countries where GiveAID is registered as a charity, donations are eligible for a deduction. Please download your official receipt from My Donations and consult your local tax advisor."
                ),
                (
                    "Can I donate completely anonymously?",
                    "I would like to support a programme without my name appearing anywhere publicly. Is anonymous donation possible on GiveAID?",
                    null
                ),
                (
                    "I am having trouble resetting my password",
                    "I requested a password reset email 30 minutes ago but have not received anything. I have also checked my spam folder. Please help.",
                    "We apologise for the inconvenience! Please try again using the 'Forgot Password' link. If the issue persists, email support@giveaid.com and our team will reset it manually within 24 hours."
                ),
                (
                    "How does GiveAID ensure funds reach the right places?",
                    "Before donating a large amount, I want to understand how GiveAID guarantees that my money goes directly to the intended programme and not lost to administrative overhead.",
                    null
                ),
                (
                    "Can I earmark my donation for a specific project within an NGO?",
                    "I want to donate to Save The Children, but only for their rural schools project, not for any other initiative. Is this possible?",
                    "Absolutely! When donating, select the specific welfare programme (e.g., 'Rural Schools for Every Child') rather than the NGO directly. Your funds will be ring-fenced for that programme alone."
                ),
                (
                    "Corporate matching gift question",
                    "My employer offers a donation matching scheme. Does GiveAID provide the necessary documentation to submit a matching gift claim to my HR department?",
                    null
                ),
                (
                    "I would like to cancel my recurring monthly donation",
                    "I set up a $50/month recurring donation three months ago. Due to personal financial changes, I need to cancel this. How do I do that without losing my donation history?",
                    "We understand completely. To cancel your recurring donation, go to My Donations > Recurring > Manage, then click 'Cancel Subscription'. Your past donation history will be fully preserved."
                ),
                (
                    "How do I change my default payment method?",
                    "I got a new bank card and would like to update my payment method so my next donation uses the new card.",
                    null
                ),
                (
                    "I want to create a fundraiser for my community",
                    "My local community was recently affected by flooding. I want to set up a fundraising page on GiveAID to collect donations from friends and family for flood relief. Is this possible?",
                    "What a wonderful initiative! We support community fundraisers. Please email fundraise@giveaid.com with your community details and flood impact description, and our team will set up a dedicated campaign page within 48 hours."
                ),
            };

            for (int i = 0; i < additionalQueries.Length; i++)
            {
                var (subj, msg, reply) = additionalQueries[i];
                queries.Add(new UserQuery
                {
                    QueryId     = Guid.CreateVersion7(),
                    UserId      = dbUsers[i % dbUsers.Count].UserId,
                    Subject     = subj,
                    MessageText = msg,
                    ReplyText   = reply,
                    CreatedAt   = DateTimeOffset.UtcNow.AddDays(-random.Next(1, 40))
                });
            }

            await context.UserQueries.AddRangeAsync(queries);
            await context.SaveChangesAsync();
        }

        // 12. Seed About Us Subpages & User Modifications
        if (!await context.AboutUsSubpages.AnyAsync())
        {
            var admin = dbUsers.FirstOrDefault(u => u.Role == UserRole.Admin);
            
            var subpages = new List<AboutUsSubpage>
            {
                new() { Slug = "our-mission", Title = "Our Mission" },
                new() { Slug = "our-story",   Title = "Our Story"   },
                new() { Slug = "our-team",    Title = "Our Team"    }
            };
            
            var newSlugs = new[] {
                "financial-transparency", "careers", "board-of-directors", "annual-reports",
                "volunteer-with-us"
            };

            var newTitles = new[] {
                "Financial Transparency", "Careers", "Board of Directors", "Annual Reports",
                "Volunteer With Us"
            };

            for (int i = 0; i < newSlugs.Length; i++)
            {
                subpages.Add(new AboutUsSubpage { Slug = newSlugs[i], Title = newTitles[i] });
            }

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

            var extraSubpageContent = new Dictionary<string, string>
            {
                ["financial-transparency"] =
                    "<section class=\"transparency-section\">" +
                    "<h2>Our Commitment to Financial Transparency</h2>" +
                    "<p>At GiveAID, we believe donors deserve to know exactly where their money goes. We publish full audited financial statements every year and maintain an overhead ratio below 12%, meaning over 88 cents of every dollar donated reaches the beneficiaries directly.</p>" +
                    "<h2>Independent Audits</h2>" +
                    "<p>Our accounts are independently audited annually by a certified public accounting firm. Audit reports are available for download in our Annual Reports section.</p>" +
                    "<h2>Programme-Level Reporting</h2>" +
                    "<p>Every active welfare programme on our platform publishes quarterly spend reports, so you can track the exact impact of your donation from contribution to outcome.</p>" +
                    "</section>",

                ["careers"] =
                    "<section class=\"careers-section\">" +
                    "<h2>Work With Us</h2>" +
                    "<p>GiveAID is a fast-growing platform with a mission-driven team. We are always looking for talented, passionate individuals who want to use their skills to make a real difference in the world.</p>" +
                    "<h2>Current Openings</h2>" +
                    "<ul><li>Senior Full-Stack Developer (Remote)</li><li>Partnerships Manager – NGO Relations</li><li>Marketing & Communications Specialist</li><li>Data Analyst – Impact Measurement</li></ul>" +
                    "<p>To apply, send your CV and a cover letter to <a href=\"mailto:careers@giveaid.com\">careers@giveaid.com</a>.</p>" +
                    "</section>",

                ["board-of-directors"] =
                    "<section class=\"board-section\">" +
                    "<h2>Board of Directors</h2>" +
                    "<p>GiveAID is governed by an independent Board of Directors drawn from the fields of international development, finance, technology, and civil society. The Board sets strategic direction and ensures accountability to our donors and beneficiaries.</p>" +
                    "<ul>" +
                    "<li><strong>Dr. Amara Diallo</strong> – Chair, former UNDP Regional Director</li>" +
                    "<li><strong>James K. Thornton</strong> – Treasurer, Chartered Accountant</li>" +
                    "<li><strong>Minh Le Van</strong> – Director, Tech for Good Ventures</li>" +
                    "<li><strong>Priya Sharma</strong> – Director, Global Health Advocate</li>" +
                    "<li><strong>Sofia Reyes</strong> – Director, Community Development Expert</li>" +
                    "</ul>" +
                    "</section>",

                ["annual-reports"] =
                    "<section class=\"reports-section\">" +
                    "<h2>Annual Reports</h2>" +
                    "<p>Our annual reports provide a comprehensive overview of GiveAID's activities, financial performance, and the measurable impact achieved across all supported programmes.</p>" +
                    "<ul>" +
                    "<li><a href=\"/downloads/GiveAID-Annual-Report-2024.pdf\">2024 Annual Report (PDF, 4.2 MB)</a></li>" +
                    "<li><a href=\"/downloads/GiveAID-Annual-Report-2023.pdf\">2023 Annual Report (PDF, 3.8 MB)</a></li>" +
                    "<li><a href=\"/downloads/GiveAID-Annual-Report-2022.pdf\">2022 Annual Report (PDF, 3.1 MB)</a></li>" +
                    "</ul>" +
                    "<p>For older reports or specific financial queries, please contact <a href=\"mailto:finance@giveaid.com\">finance@giveaid.com</a>.</p>" +
                    "</section>",

                ["volunteer-with-us"] =
                    "<section class=\"volunteer-section\">" +
                    "<h2>Make an Impact Beyond Donating</h2>" +
                    "<p>Volunteering with GiveAID is one of the most powerful ways you can drive change. Whether you have a few hours a week or want to dedicate more time, there is a meaningful role for everyone.</p>" +
                    "<h2>Volunteer Opportunities</h2>" +
                    "<ul>" +
                    "<li><strong>Event Volunteers</strong> – Help organise and run our fundraising events and community outreach days.</li>" +
                    "<li><strong>Digital Skills Trainers</strong> – Share your expertise in coding, design, or digital marketing with youth in our programmes.</li>" +
                    "<li><strong>Field Support</strong> – Work directly alongside our NGO partners in programme delivery on the ground.</li>" +
                    "<li><strong>Mentors</strong> – Guide young people in our Youth Leadership Academy programme.</li>" +
                    "</ul>" +
                    "<p>Interested? Fill in our <a href=\"/volunteer-apply\">Volunteer Application Form</a> and our team will be in touch within 5 business days.</p>" +
                    "</section>"
            };

            for (int i = 3; i < subpages.Count; i++)
            {
                var slug = subpages[i].Slug;
                modifications.Add(new UserModification
                {
                    ModificationId = Guid.CreateVersion7(),
                    SubpageId      = subpages[i].SubpageId,
                    UserId         = admin?.UserId,
                    HtmlContent    = extraSubpageContent.TryGetValue(slug, out var html)
                                         ? html
                                         : $"<section><h2>{subpages[i].Title}</h2><p>Content coming soon.</p></section>",
                    CreatedAt      = DateTimeOffset.UtcNow.AddDays(-random.Next(1, 90))
                });
            }

            await context.UserModifications.AddRangeAsync(modifications);
            await context.SaveChangesAsync();
        }
    }
}
