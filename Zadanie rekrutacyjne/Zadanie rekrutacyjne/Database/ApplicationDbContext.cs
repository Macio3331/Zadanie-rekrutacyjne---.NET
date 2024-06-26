﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Serilog;
using Zadanie_rekrutacyjne.Models;

namespace Zadanie_rekrutacyjne.Database
{
    /// <summary>
    /// Class representing a database.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Constructor used for setting options (getting connection) to a database.
        /// </summary>
        /// <param name="options">Object of options.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            try
            {
                var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (databaseCreator != null)
                {
                    if (!databaseCreator.CanConnect()) databaseCreator.Create();
                    if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Could not create a database.");
            }
        }
        /// <summary>
        /// Field representing a table of tags.
        /// </summary>
        public DbSet<TagModel> Tags { get; set; }
        /// <summary>
        /// Method used for configuring the table of tags (setting auto-generated value of ID, Name and Count fields as required).
        /// </summary>
        /// <param name="modelBuilder">Object building a database model.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TagModel>().Property(t => t.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<TagModel>().Property(t => t.Name).IsRequired();
            modelBuilder.Entity<TagModel>().Property(t => t.Count).IsRequired();
        }
    }
}
