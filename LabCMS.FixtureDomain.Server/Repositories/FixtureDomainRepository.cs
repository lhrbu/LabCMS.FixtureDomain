using LabCMS.FixtureDomain.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace LabCMS.FixtureDomain.Server.Repositories;
public class FixtureDomainRepository:DbContext
{
    private readonly IConfiguration _configuration;
    public FixtureDomainRepository(DbContextOptions<FixtureDomainRepository> options,
        IConfiguration configuration)
            : base(options) { _configuration = configuration; }
    public DbSet<Fixture> Fixtures => Set<Fixture>();
    public DbSet<FixtureEventInDatabase> FixtureEventsInDatabase => Set<FixtureEventInDatabase>();
    public DbSet<Role> Roles => Set<Role>();
    public IEnumerable<Role> AdminRoles
    {
        get {
            string[] adminUsersId = _configuration.GetValue<string[]>("AdminUsersId");
            return Roles.Where(item => adminUsersId.Contains(item.UserId));
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Fixture>()
            .HasKey(item => item.No);
        modelBuilder.Entity<Fixture>()
            .Property(item => item.No)
            .ValueGeneratedNever();
        modelBuilder.Entity<Fixture>()
            .Property(item => item.TestField)
            .HasConversion<string>();
        modelBuilder.Entity<Fixture>()
            .Property(item=>item.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Fixture>()
            .HasIndex(item => item.TestField);

        modelBuilder.Entity<Fixture>()
            .HasIndex(item => item.ProjectShortName);

        modelBuilder.Entity<FixtureEventInDatabase>()
            .HasIndex(item => item.FixtureNo);
        modelBuilder.Entity<FixtureEventInDatabase>()
            .HasIndex(item => item.ContentTypeFullName);
    }
}
