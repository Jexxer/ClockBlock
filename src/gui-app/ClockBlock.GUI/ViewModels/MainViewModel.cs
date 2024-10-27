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
    /// <summary>
    /// The MainViewModel class manages the configuration settings, saves data to the database, 
    /// and provides data binding and validation for the WPF GUI.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Fields

        private readonly ClockBlockContext _context;
        private ConfigurationDto _configDto = new ConfigurationDto { WorkingHoursStart = "09:00", WorkingHoursEnd = "17:00" };
        private string _workingHoursStart = "09:00";
        private string _workingHoursEnd = "17:00";
        private bool _isSaving;
        private string _statusMessage = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the start time for working hours.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the end time for working hours.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the status of the save operation.
        /// </summary>
        public bool IsSaving
        {
            get => _isSaving;
            set
            {
                _isSaving = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the status message displayed to the user.
        /// </summary>
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Represents the configuration data model.
        /// </summary>
        public AppConfig Config { get; set; } = new AppConfig();

        /// <summary>
        /// Command to save the configuration settings asynchronously.
        /// </summary>
        public RelayCommand SaveConfigCommand { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor for production, initializes with a new database context.
        /// </summary>
        public MainViewModel() : this(new ClockBlockContext()) { }

        /// <summary>
        /// Constructor for dependency injection/testing, accepts an existing database context.
        /// </summary>
        /// <param name="context">The ClockBlockContext database context.</param>
        public MainViewModel(ClockBlockContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
            LoadConfiguration();
            SaveConfigCommand = new RelayCommand(async () => await SaveConfigAsync(), () => !IsSaving);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the configuration from the database or sets default values if none exist.
        /// </summary>
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

        /// <summary>
        /// Asynchronously saves the current configuration settings to the database.
        /// Validates time format before saving and provides user feedback.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation.</returns>
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

        /// <summary>
        /// Validates whether the provided time string matches the required "HH:mm" format.
        /// </summary>
        /// <param name="time">The time string to validate.</param>
        /// <returns>True if the time is valid, otherwise false.</returns>
        private bool IsValidTimeFormat(string time)
        {
            return DateTime.TryParseExact(time, "HH:mm", null, System.Globalization.DateTimeStyles.None, out _);
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Notifies the UI when a property value changes.
        /// </summary>
        /// <param name="name">The name of the changed property.</param>
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
