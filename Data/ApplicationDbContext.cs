using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vitalia.Models;

namespace Vitalia.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Seller> Sellers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Client>().ToTable("Clients")
                .HasOne(c => c.User)
                .WithOne()
                .HasForeignKey<Client>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Doctor>().ToTable("Doctors")
                .HasOne(d => d.User)
                .WithOne()
                .HasForeignKey<Doctor>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Seller>().ToTable("Sellers")
                .HasOne(s => s.User)
                .WithOne()
                .HasForeignKey<Seller>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}