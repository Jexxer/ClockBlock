using Microsoft.EntityFrameworkCore;
using ClockBlock.GUI.Models.DTOs;
using System.IO;

namespace ClockBlock.GUI.Data
{
    /// <summary>
    /// The ClockBlockContext class provides access to the database for configuration storage.
    /// Configures the database to use SQLite as a persistent storage or in-memory database for testing.
    /// </summary>
    public class ClockBlockContext : DbContext
    {
        #region DbSets

        /// <summary>
        /// Gets or sets the Configurations table for storing application settings.
        /// </summary>
        public DbSet<ConfigurationDto> Configurations { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor for production use, intended for use without dependency injection.
        /// </summary>
        public ClockBlockContext() : base() { }

        /// <summary>
        /// Constructor for dependency injection, allowing configuration of DbContextOptions externally.
        /// Typically used for testing or specialized configuration.
        /// </summary>
        /// <param name="options">The DbContextOptions to configure the database connection.</param>
        public ClockBlockContext(DbContextOptions<ClockBlockContext> options) : base(options) { }

        #endregion

        #region Methods

        /// <summary>
        /// Configures the database to use SQLite for persistent storage if no configuration is provided.
        /// Only invoked if DbContextOptions have not been externally configured.
        /// </summary>
        /// <param name="optionsBuilder">The options builder used to configure the database context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Define the path for the SQLite database file in local application data
                var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ClockBlock", "ClockBlock.db");
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath) ?? Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

                // Configure the options builder to use SQLite with the specified database path
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
        }

        #endregion
    }
}
