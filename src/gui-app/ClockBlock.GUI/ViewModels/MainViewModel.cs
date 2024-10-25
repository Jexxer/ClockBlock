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
        public AppConfig Config
        {
            get => _config;
            set
            {
                _config = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand SaveConfigCommand { get; }

        // Constructor
        public MainViewModel()
        {
            // Load the config from the file or set default values
            Config = LoadConfig();
            SaveConfigCommand = new RelayCommand(SaveConfig);
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
