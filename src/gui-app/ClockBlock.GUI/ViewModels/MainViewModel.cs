using ClockBlock.GUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClockBlock.GUI.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private AppConfig _config = new();
        private bool _isSaving;
        private string _statusMessage = String.Empty;
        private string _workingHoursStart;
        private string _workingHoursEnd;

        public string WorkingHoursStart
        {
            get => _workingHoursStart;
            set
            {
                _workingHoursStart = value;
                Config.WorkingHoursStart = value;
                OnPropertyChanged();
            }
        }

        public string WorkingHoursEnd
        {
            get => _workingHoursEnd;
            set
            {
                _workingHoursEnd = value;
                Config.WorkingHoursEnd = value;
                OnPropertyChanged();
            }
        }

        public bool IsSaving
        {
            get => _isSaving;
            set
            {
                _isSaving = value;
                OnPropertyChanged();
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        public AppConfig Config
        {
            get => _config;
            set
            {
                _config = value;
                OnPropertyChanged();
            }
        }

        private bool IsValidTimeFormat(string time)
        {
            return DateTime.TryParseExact(time, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }

        public RelayCommand SaveConfigCommand { get; }

        // Constructor
        public MainViewModel()
        {
            Config = LoadConfig();
            _workingHoursStart = Config.WorkingHoursStart;
            _workingHoursEnd = Config.WorkingHoursEnd;
            SaveConfigCommand = new RelayCommand(async () => await SaveConfigAsync(), () => !IsSaving);
        }

        public async Task SaveConfigAsync()
        {
            // Perform validation before saving
            if (!IsValidTimeFormat(WorkingHoursStart) || !IsValidTimeFormat(WorkingHoursEnd))
            {
                StatusMessage = "Please correct the time format before saving.\nExample: 21:00";
                return;
            }

            IsSaving = true;
            StatusMessage = "Saving...";

            await Task.Run(() =>
            {
                var json = JsonSerializer.Serialize(Config);
                File.WriteAllText("config.json", json);
            });

            StatusMessage = "Configuration saved successfully!";
            IsSaving = false;

            // Reset message after a short delay
            await Task.Delay(2000);
            StatusMessage = string.Empty;
        }

        // Load config from file
        private AppConfig LoadConfig()
        {
            if (File.Exists("config.json"))
            {
                Debug.WriteLine("Loading config from file.");
                var json = File.ReadAllText("config.json");
                return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
            }
            StatusMessage = "Configuration file not found. Using default values.";
            return new AppConfig();
        }

        // Save config to file
        public void SaveConfig()
        {
            var json = JsonSerializer.Serialize(Config);
            File.WriteAllText("config.json", json);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        // Implement INotifyPropertyChanged for data binding
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
