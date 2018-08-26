using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Kait.Support
{
    class RunCommand : ICommand
    {
        private Action<object> ExecuteCommand;
        private Func<bool> CanExecuteCommand;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RunCommand(Action<object> executeAction)
        {
            ExecuteCommand = executeAction;
            CanExecuteCommand = null;
        }
        public RunCommand(Action<object> executeAction,Func<bool> canExecuteAction)
        {
            ExecuteCommand = executeAction;
            CanExecuteCommand = canExecuteAction;

        }

       

        public bool CanExecute(object parameter)
        {
            if (CanExecuteCommand!=null)
            {
                return CanExecuteCommand();
            }
            else
            {
                return true;
            }

        }

        public void Execute(object parameter)
        {
            ExecuteCommand(parameter);
        }
    }
}
