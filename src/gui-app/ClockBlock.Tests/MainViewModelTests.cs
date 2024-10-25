using ClockBlock.GUI.Models;
using ClockBlock.GUI.ViewModels;
using System.IO;
using System.Text.Json;
using Xunit;

namespace ClockBlock.Tests
{
    public class MainViewModelTests
    {
        [Fact]
        public void LoadConfig_ShouldLoadDefaultConfig_WhenConfigFileDoesNotExist()
        {
            // Arrange
            if (File.Exists("config.json"))
            {
                File.Delete("config.json"); // Ensure no config file exists
            }

            var viewModel = new MainViewModel();

            // Act
            var config = viewModel.Config;

            // Assert
            Assert.Equal("09:00", config.WorkingHoursStart);
            Assert.Equal("17:00", config.WorkingHoursEnd);
        }

        [Fact]
        public void SaveConfig_ShouldCreateConfigFile_WithCorrectValues()
        {
            // Arrange
            var viewModel = new MainViewModel
            {
                Config = new AppConfig
                {
                    WorkingHoursStart = "08:00",
                    WorkingHoursEnd = "18:00"
                }
            };

            // Act
            viewModel.SaveConfig();

            // Assert
            Assert.True(File.Exists("config.json"));

            var savedConfig = JsonSerializer.Deserialize<AppConfig>(File.ReadAllText("config.json"));
            Assert.Equal("08:00", savedConfig?.WorkingHoursStart);
            Assert.Equal("18:00", savedConfig?.WorkingHoursEnd);

            // Clean up
            File.Delete("config.json");
        }

        [Fact]
        public void SaveConfig_ShouldOverwriteExistingConfigFile()
        {
            // Arrange
            var viewModel = new MainViewModel();
            viewModel.Config = new AppConfig { WorkingHoursStart = "10:00", WorkingHoursEnd = "16:00" };
            viewModel.SaveConfig();

            // Act
            viewModel.Config.WorkingHoursStart = "07:00";
            viewModel.Config.WorkingHoursEnd = "15:00";
            viewModel.SaveConfig();

            // Assert
            var savedConfig = JsonSerializer.Deserialize<AppConfig>(File.ReadAllText("config.json"));
            Assert.Equal("07:00", savedConfig?.WorkingHoursStart);
            Assert.Equal("15:00", savedConfig?.WorkingHoursEnd);

            // Clean up
            File.Delete("config.json");
        }
    }
}
