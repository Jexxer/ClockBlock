using ClockBlock.GUI.Models;
using ClockBlock.GUI.Models.DTOs;
using ClockBlock.GUI.Data;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ClockBlock.GUI.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ClockBlockContext _context;
        private ConfigurationDto _configDto = new ConfigurationDto { WorkingHoursStart = "09:00", WorkingHoursEnd = "17:00" };

        private string _workingHoursStart = "09:00";
        private string _workingHoursEnd = "17:00";
        private bool _isSaving;
        private string _statusMessage = string.Empty;

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

        public AppConfig Config { get; set; } = new AppConfig();
        public RelayCommand SaveConfigCommand { get; }

        // Default constructor for production
        public MainViewModel() : this(new ClockBlockContext()) { }

        // Constructor for dependency injection/testing
        public MainViewModel(ClockBlockContext context)
        {
            _context = context; // Use the provided context without reinitializing
            _context.Database.EnsureCreated();
            LoadConfiguration();
            SaveConfigCommand = new RelayCommand(async () => await SaveConfigAsync(), () => !IsSaving);
        }

        private void LoadConfiguration()
        {
            _configDto = _context.Configurations.FirstOrDefault() ?? new ConfigurationDto
            {
                WorkingHoursStart = "09:00",
                WorkingHoursEnd = "17:00"
            };

            // Sync DTO with ViewModel properties
            WorkingHoursStart = _configDto.WorkingHoursStart;
            WorkingHoursEnd = _configDto.WorkingHoursEnd;
        }

        public async Task SaveConfigAsync()
        {
            // Validate before saving
            if (!IsValidTimeFormat(WorkingHoursStart) || !IsValidTimeFormat(WorkingHoursEnd))
            {
                StatusMessage = "Please correct the time format before saving.\nExample: 21:00";
                return;
            }

            IsSaving = true;
            StatusMessage = "Saving...";

            // Update DTO and save to database
            _configDto.WorkingHoursStart = WorkingHoursStart;
            _configDto.WorkingHoursEnd = WorkingHoursEnd;

            _context.Configurations.Update(_configDto);
            await _context.SaveChangesAsync();

            StatusMessage = "Configuration saved successfully!";
            IsSaving = false;

            // Reset message after a short delay
            await Task.Delay(2000);
            StatusMessage = string.Empty;
        }

        private bool IsValidTimeFormat(string time)
        {
            return DateTime.TryParseExact(time, "HH:mm", null, System.Globalization.DateTimeStyles.None, out _);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
