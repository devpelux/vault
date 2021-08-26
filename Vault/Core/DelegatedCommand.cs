using System;
using System.Windows.Input;

namespace Vault.Core
{
    public class DelegatedCommand : ICommand
    {
        public Predicate<object> CanExecuteDelegate { get; }

        public Action<object> ExecuteDelegate { get; }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


        public DelegatedCommand(Action<object> execute)
        {
            ExecuteDelegate = execute;
        }

        public DelegatedCommand(Action<object> execute, Predicate<object> canExecute)
        {
            ExecuteDelegate = execute;
            CanExecuteDelegate = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteDelegate == null || CanExecuteDelegate(parameter);
        }

        public void Execute(object parameter)
        {
            ExecuteDelegate?.Invoke(parameter);
        }
    }
}
