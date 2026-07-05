using GiveAID.Models;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Data;

public class AppDbContext(DbContextOptions<AppDbContext> opts): DbContext(opts)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<AboutSubpage> AboutSubpages => Set<AboutSubpage>();
    public DbSet<Member> Members => Set<Member>();
};