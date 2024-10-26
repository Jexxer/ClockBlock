using ClockBlock.GUI.Data;
using ClockBlock.GUI.Models;
using ClockBlock.GUI.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace ClockBlock.Tests
{
    [Collection("NonParallelTests")]
    public class MainViewModelTests
    {
        private ClockBlockContext CreateInMemoryContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<ClockBlockContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;

            var context = new ClockBlockContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public void LoadConfig_ShouldLoadDefaultConfig_WhenDatabaseIsEmpty()
        {
            // Arrange
            var context = CreateInMemoryContext("TestDb_LoadConfig_Defaults");
            var viewModel = new MainViewModel(context);

            // Act
            var config = viewModel.Config;

            // Assert
            Assert.Equal("09:00", config.WorkingHoursStart);
            Assert.Equal("17:00", config.WorkingHoursEnd);
        }

        [Fact]
        public async Task SaveConfig_ShouldSaveConfig_WithCorrectValues()
        {
            // Arrange
            var context = CreateInMemoryContext("TestDb_SaveConfig_CorrectValues");
            var viewModel = new MainViewModel(context)
            {
                WorkingHoursStart = "08:00",
                WorkingHoursEnd = "18:00"
            };

            // Act
            await viewModel.SaveConfigAsync();

            // Assert
            var savedConfig = await context.Configurations.FirstOrDefaultAsync();
            Assert.NotNull(savedConfig);
            Assert.Equal("08:00", savedConfig?.WorkingHoursStart);
            Assert.Equal("18:00", savedConfig?.WorkingHoursEnd);
        }

        [Fact]
        public async Task SaveConfig_ShouldOverwriteExistingConfig()
        {
            // Arrange
            var context = CreateInMemoryContext("TestDb_SaveConfig_Overwrite");
            var viewModel = new MainViewModel(context)
            {
                WorkingHoursStart = "10:00",
                WorkingHoursEnd = "16:00"
            };

            // Initial save
            await viewModel.SaveConfigAsync();

            // Act - Modify and save again
            viewModel.WorkingHoursStart = "07:00";
            viewModel.WorkingHoursEnd = "15:00";
            await viewModel.SaveConfigAsync();

            // Assert
            var savedConfig = await context.Configurations.FirstOrDefaultAsync();
            Assert.NotNull(savedConfig);
            Assert.Equal("07:00", savedConfig?.WorkingHoursStart);
            Assert.Equal("15:00", savedConfig?.WorkingHoursEnd);
        }

        [Fact]
        public async Task SaveConfigAsync_ShouldSetStatusMessageDuringAndAfterSave()
        {
            // Arrange
            var context = CreateInMemoryContext("TestDb_SaveConfig_StatusMessage");
            var viewModel = new MainViewModel(context);

            // Act
            await viewModel.SaveConfigAsync();

            // Assert - StatusMessage should reset after delay
            Assert.Equal(string.Empty, viewModel.StatusMessage);
        }

        [Fact]
        public async Task SaveConfigAsync_ShouldNotSaveConfig_WhenTimeFormatIsInvalid()
        {
            // Arrange
            var context = CreateInMemoryContext("TestDb_SaveConfig_InvalidFormat");
            var viewModel = new MainViewModel(context)
            {
                WorkingHoursStart = "10:00",
                WorkingHoursEnd = "16:00"
            };

            // Act - Set invalid time format and attempt to save
            viewModel.WorkingHoursStart = "10:00:00"; // Invalid format
            await viewModel.SaveConfigAsync();

            // Assert - Save should be blocked by validation
            Assert.Equal("Please correct the time format before saving.\nExample: 21:00", viewModel.StatusMessage);
            Assert.Null(await context.Configurations.FirstOrDefaultAsync()); // No entry should be saved
        }
    }

    [CollectionDefinition("NonParallelTests", DisableParallelization = true)]
    public class NonParallelTestsCollection : ICollectionFixture<NonParallelTestsCollection> { }

}
