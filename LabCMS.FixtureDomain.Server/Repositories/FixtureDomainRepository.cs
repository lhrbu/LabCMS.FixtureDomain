using LabCMS.FixtureDomain.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace LabCMS.FixtureDomain.Server.Repositories;
public class FixtureDomainRepository:DbContext
{
    public FixtureDomainRepository(DbContextOptions<FixtureDomainRepository> options)
            : base(options) { }
    public DbSet<Fixture> Fixtures => Set<Fixture>();
    public DbSet<FixtureEventInDatabase> FixtureEventsInDatabase => Set<FixtureEventInDatabase>();
    public DbSet<Role> Roles => Set<Role>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
