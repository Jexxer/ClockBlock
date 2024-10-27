# ClockBlock (WIP)

ClockBlock is a productivity tool designed to help users stay focused by blocking access to specific applications during defined working hours. It consists of a Windows background service and a WPF-based GUI application for configuration and viewing usage statistics.

## Current Features
- **Configuration Management**: Set and save working hours to block access to certain applications during specified times.
- **MVVM Architecture**: Organized code structure in WPF using the MVVM pattern for scalability and testability.
- **Unit Tests**: Basic tests for configuration loading and saving to ensure robust functionality.

## Project Structure
- **/gui-app**: Contains the WPF GUI application, including:
  - **Models**: Application configuration model.
  - **ViewModels**: Handles data binding and logic between the UI and data models.
  - **Views**: WPF XAML files for the GUI interface.
  - **Tests**: Unit tests for validating functionality.
- **/service**: Will contain the Rust background service responsible for blocking applications outside working hours.

## Roadmap

### 1. WPF GUI
   - **Goal**: Build a user-friendly interface for configuration and statistics.
   - **Steps**:
     - ✅ Set up the initial MVVM project structure in WPF.
     - ✅ Create `MainViewModel` for managing configuration settings and view logic.
     - ✅ Implement `IDataErrorInfo` for validating inputs, like working hours.
     - ✅ Create a `SettingsView` with fields for working hours and save functionality.
     - ⬜ Develop a statistics view to display blocked application history.
     - ⬜ Add user notifications for blocked applications.
     - ⬜ Refine UI components, tooltips, and layout for usability.

### 2. Creating Database
   - **Goal**: Design and initialize a database to store configuration, blocked applications, and user statistics.
   - **Steps**:
     - ⬜ Define database schema (tables for configuration, blocked apps, session history).
     - ⬜ Choose and configure a lightweight local database solution (e.g., SQLite).
     - ⬜ Set up database scripts to create initial schema.
     - ⬜ Ensure proper indexing and optimize queries for statistics and configurations.

### 3. Implementing Entity Framework (EF) Core
   - **Goal**: Use EF Core for ORM functionality to interact with the database seamlessly.
   - **Steps**:
     - ⬜ Install EF Core and set up initial configurations in the WPF project.
     - ⬜ Define `DbContext` and configure entities (Configuration, BlockedApp, Session).
     - ⬜ Implement repository or service layer to manage CRUD operations.
     - ⬜ Seed initial data as needed for testing.
     - ⬜ Test database interactions for performance and accuracy.

### 4. Background Service (Rust)
   - **Goal**: Create a Rust-based background service to block application usage outside defined working hours.
   - **Steps**:
     - ⬜ Set up the Rust project with a basic structure.
     - ⬜ Implement application monitoring to detect active processes.
     - ⬜ Create a configuration file or API to read allowed working hours.
     - ⬜ Implement blocking logic to terminate or prevent blocked applications.
     - ⬜ Add communication with the WPF application for real-time status updates.
     - ⬜ Test and optimize service reliability, performance, and error handling.

### TESTS. Writing Tests for GUI, Database, EF Core, and Service
   - **Goal**: Ensure application robustness through comprehensive unit and integration tests.
   - **Steps**:
     - ✅ Write unit tests for `MainViewModel` (input validation, save/load configuration).
     - ⬜ Write unit tests for database models and CRUD operations using EF Core.
     - ⬜ Implement integration tests for WPF data binding and interaction flows.
     - ⬜ Write tests for statistics tracking and data persistence.
     - ⬜ Validate and debug with automated tests to ensure full feature coverage.

## Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/ClockBlock.git
   ```
2. Open the solution in Visual Studio.
3. Build the project to restore dependencies and run the application.
