using Microsoft.EntityFrameworkCore;
using DataLayer.Entities;

namespace DataLayer.AppDbContext
{

    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
        }

        public DbSet<Entity> Entities { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Date> Dates { get; set; }
        public DbSet<Name> Names { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entity>()
                .HasMany(e => e.Addresses)
                .WithOne(a => a.Entity)
                .HasForeignKey(a => a.EntityId)
                .IsRequired();

            modelBuilder.Entity<Entity>()
                .HasMany(e => e.Dates)
                .WithOne(d=>d.Entity)
                .HasForeignKey(d => d.EntityId)
                .IsRequired();

            modelBuilder.Entity<Entity>()
                .HasMany(e => e.Names)
                .WithOne(n=>n.Entity)
                .HasForeignKey(n => n.EntityId)
                .IsRequired();
        }
    }
}
