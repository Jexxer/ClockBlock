using Microsoft.EntityFrameworkCore;
using ClockBlock.GUI.Models.DTOs;
using System.IO;

namespace ClockBlock.GUI.Data
{
    public class ClockBlockContext : DbContext
    {
        public DbSet<ConfigurationDto> Configurations { get; set; }

        // Default constructor for production use
        public ClockBlockContext() : base() { }

        // Constructor for testing, accepting DbContextOptions
        public ClockBlockContext(DbContextOptions<ClockBlockContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Only configure if optionsBuilder has not been configured elsewhere
            if (!optionsBuilder.IsConfigured)
            {
                var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ClockBlock", "ClockBlock.db");
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath) ?? Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
        }
    }
}
