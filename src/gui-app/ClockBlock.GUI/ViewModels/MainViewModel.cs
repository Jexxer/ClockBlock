using ClockBlock.GUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public AppConfig Config
        {
            get => _config;
            set
            {
                _config = value;
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

        public RelayCommand SaveConfigCommand { get; }

        // Constructor
        public MainViewModel()
        {
            // Load the config from the file or set default values
            Config = LoadConfig();
            SaveConfigCommand = new RelayCommand(async () => await SaveConfigAsync(), () => !IsSaving);
        }

        private async Task SaveConfigAsync()
        {
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
        private static AppConfig LoadConfig()
        {
            if (File.Exists("config.json"))
            {
                var json = File.ReadAllText("config.json");
                return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
            }
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
