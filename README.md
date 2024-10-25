# ClockBlock

ClockBlock is a productivity tool designed to help users stay focused by blocking access to specific applications during defined working hours. It consists of a Windows background service and a WPF-based GUI application for configuration and viewing usage statistics.

## Current Features
- **Configuration Management**: Set and save working hours to block access to certain applications during specified times.
- **MVVM Architecture**: Organized code structure in WPF using the MVVM pattern for scalability and testability.
- **Unit Tests**: Basic tests for configuration loading and saving to ensure robust functionality.

## Project Structure
- **Models**: Holds the application’s configuration model.
- **ViewModels**: Handles data binding and logic between the UI and data models.
- **Views**: Contains WPF XAML files for the GUI interface.
- **Tests**: Unit tests for validating functionality.

## Roadmap
- ✅ Initial project structure with MVVM in WPF
- ✅ Configuration model and handling
- ✅ Unit tests for `MainViewModel`
- ✅ Input validation for working hours
- ⬜ Integration with Rust background service
- ⬜ Enhanced UI and notification system

## Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/ClockBlock.git
