namespace GiveAID.Models;

public static class MockData
{
    public static readonly string[] Amounts = ["Rs. 500", "Rs. 1,000", "Rs. 2,500", "Rs. 5,000", "Rs. 10,000", "Other Amount"];
    
    public static readonly string[] Causes = ["Children", "Education", "Health", "Disabled", "Women", "Environment", "General Fund"];
    
    public static readonly List<Partner> Partners = new()
    {
        new Partner { Initials = "GT", Name = "GlobalTech Corp", Sector = "Technology" },
        new Partner { Initials = "MB", Name = "Meridian Bank", Sector = "Finance" },
        new Partner { Initials = "SL", Name = "SunLife Pharma", Sector = "Healthcare" },
        new Partner { Initials = "BI", Name = "BuildWell Infra", Sector = "Construction" },
        new Partner { Initials = "RL", Name = "Retail Link", Sector = "Retail" },
        new Partner { Initials = "EF", Name = "EduFirst", Sector = "Education" }
    };

    public static readonly List<Ngo> InitialNgos = new()
    {
        new Ngo { Name = "HelpFirst India", Focus = "Child Education", Location = "New Delhi", Since = "2008" },
        new Ngo { Name = "VisionAble Trust", Focus = "Disability Support", Location = "Mumbai", Since = "2011" },
        new Ngo { Name = "GreenEarth Foundation", Focus = "Environment", Location = "Bangalore", Since = "2015" },
        new Ngo { Name = "WomenEmpower Network", Focus = "Skill Training", Location = "Pune", Since = "2010" }
    };

    public static readonly List<Program> Programs = new()
    {
        new Program { Tag = "Education", Title = "Education for All", Desc = "Scholarships, study kits, and mentoring for 12,000+ underprivileged children across 6 states.", Count = "12,400 beneficiaries", Icon = "graduation-cap" },
        new Program { Tag = "Health Care", Title = "Community Health Drive", Desc = "Free medical camps, vaccinations, and mental health support for rural and semi-urban communities.", Count = "8,900 patients served", Icon = "heart" },
        new Program { Tag = "Special Needs", Title = "Privileged Children", Desc = "Specialized therapy, inclusive schooling, and assistive devices for specially-challenged children.", Count = "2,100 children", Icon = "baby" },
        new Program { Tag = "Environment", Title = "Green Earth Initiative", Desc = "Large-scale tree plantation drives and water conservation workshops in arid regions.", Count = "50,000+ trees planted", Icon = "tree-pine" },
        new Program { Tag = "Women", Title = "Women in Tech", Desc = "Digital literacy and vocational training for women from marginalized communities.", Count = "4,500 trained", Icon = "user-check" }
    };

    public static readonly List<GalleryImage> GalleryImages = new()
    {
        new GalleryImage { Id = "1488521787991-ed7bbaae773c", Tag = "Health", Caption = "Annual Health Camp — June 2024" },
        new GalleryImage { Id = "1532629345422-7515f3d16bb6", Tag = "Education", Caption = "Children Education Drive — March 2024" },
        new GalleryImage { Id = "1542810634-71277d2d82b5", Tag = "Women", Caption = "Women Skill Training — Feb 2024" },
        new GalleryImage { Id = "1601933470096-0e34634ffcde", Tag = "Environment", Caption = "Tree Plantation — Earth Day 2024" },
        new GalleryImage { Id = "1576267423048-15c0040fec78", Tag = "Disabled", Caption = "Assistive Device Distribution — Jan 2024" },
        new GalleryImage { Id = "1469571433402-8be3f4ff0022", Tag = "Youth", Caption = "Youth Leadership Workshop — Dec 2023" }
    };
}

public class Partner
{
    public string Initials { get; set; } = "";
    public string Name { get; set; } = "";
    public string Sector { get; set; } = "";
}

public class Ngo
{
    public string Name { get; set; } = "";
    public string Focus { get; set; } = "";
    public string Location { get; set; } = "";
    public string Since { get; set; } = "";
}

public class Program
{
    public string Tag { get; set; } = "";
    public string Title { get; set; } = "";
    public string Desc { get; set; } = "";
    public string Count { get; set; } = "";
    public string Icon { get; set; } = "";
}

public class GalleryImage
{
    public string Id { get; set; } = "";
    public string Tag { get; set; } = "";
    public string Caption { get; set; } = "";
}
