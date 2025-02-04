using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SelfHostedSuperViser.Commands
{
    public class RelayCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        private Action<object> _Execute { get; set; } // Action: Object -> method. An action is performed. Nothing is returned.
        private Predicate<object> _CanExecute { get; set; } // Predicate: Object -> method. A calculation is done. A bool is returned.

        public RelayCommand(Action<object> executeMethod, Predicate<object> canExecuteMethod)
        {
            ArgumentNullException.ThrowIfNull(executeMethod);
            ArgumentNullException.ThrowIfNull(canExecuteMethod);

            _Execute = executeMethod;
            _CanExecute = canExecuteMethod;
        }

        public bool CanExecute(object? parameter)
        {
            return _CanExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            _Execute(parameter);
        }
    }
}
