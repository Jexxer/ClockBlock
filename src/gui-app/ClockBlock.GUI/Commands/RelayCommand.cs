using System;
using System.Windows.Input;

namespace ClockBlock.GUI.ViewModels
{
    /// <summary>
    /// The RelayCommand class provides a reusable implementation of ICommand
    /// to handle command execution and enable or disable command functionality.
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Fields

        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the RelayCommand class.
        /// </summary>
        /// <param name="execute">The action to execute when the command is invoked.</param>
        /// <param name="canExecute">A function that determines whether the command can execute.</param>
        /// <exception cref="ArgumentNullException">Thrown when the execute action is null.</exception>
        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        #endregion

        #region ICommand Members

        /// <summary>
        /// Occurs when changes occur that affect whether the command should execute.
        /// Initialized to an empty delegate to avoid null checks.
        /// </summary>
        public event EventHandler? CanExecuteChanged = delegate { };

        /// <summary>
        /// Determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command. Not used in this implementation.</param>
        /// <returns>True if the command can execute, otherwise false.</returns>
        public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

        /// <summary>
        /// Executes the command action if the command can execute.
        /// </summary>
        /// <param name="parameter">Data used by the command. Not used in this implementation.</param>
        public void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                _execute();
            }
        }

        /// <summary>
        /// Raises the CanExecuteChanged event to signal that the result of CanExecute may have changed.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
