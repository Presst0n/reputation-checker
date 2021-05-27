using System;
using System.Windows.Input;

namespace RepChecker.Core
{
    public class RelayCommand<T> : ICommand
    {
        //private Action<object> _execute;
        //private Func<object, bool> _canExecute;

        //public event EventHandler CanExecuteChanged
        //{
        //    add { CommandManager.RequerySuggested += value; }
        //    remove { CommandManager.RequerySuggested -= value; }
        //}

        //public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        //{
        //    _execute = execute;
        //    _canExecute = canExecute;
        //}

        //public bool CanExecute(object parameter)
        //{
        //    return _canExecute == null || _canExecute(parameter);
        //}

        //public void Execute(object parameter)
        //{
        //    _execute(parameter);
        //}

        protected readonly Action<T> _execute;
        protected readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute), "Command must have execute action provided");
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute((T)parameter);

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }

    public class RelayCommand : RelayCommand<object>
    {
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
            : base(execute, canExecute)
        { }
    }

}

