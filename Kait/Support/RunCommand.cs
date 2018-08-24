using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Kait.Support
{
    class RunCommand : ICommand
    {
        private Action ExecuteCommand;
        public event EventHandler CanExecuteChanged;

        public RunCommand(Action executeAction)
        {
            ExecuteCommand = executeAction;
            
        }

        public bool CanExecute(object parameter)
        {
       
            return true;
            
        }

        public void Execute(object parameter)
        {
            ExecuteCommand();
        }
    }
}
