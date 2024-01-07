using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Db
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {            
        }
        public DbSet<City> Cities { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<Donor> Donors { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<BloodRequest> BloodRequests { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BloodRequestDonation>()
                  .HasKey(m => new { m.BloodRequestId, m.DonationId });
        }
        public DbSet<Api.Models.BloodRequestDonation> BloodRequestDonation { get; set; } = default!;
    }
}
