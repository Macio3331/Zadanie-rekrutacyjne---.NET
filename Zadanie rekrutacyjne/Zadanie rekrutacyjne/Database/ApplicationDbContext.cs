using Microsoft.EntityFrameworkCore;
using Zadanie_rekrutacyjne.Models;

namespace Zadanie_rekrutacyjne.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TagModel> Tags { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TagModel>().Property(t => t.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<TagModel>().Property(t => t.Name).IsRequired();
            modelBuilder.Entity<TagModel>().Property(t => t.Count).IsRequired();
        }
    }
}
