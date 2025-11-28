using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Presenter.ViewModel
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private event Action _execute;
        private event Func<bool> _canExecute;

        public RelayCommand(Action Execute)
        {
            _execute = Execute;
        }

        public RelayCommand(Action Execute, Func<bool> CanExecute)
        {
            _execute = Execute;
            _canExecute = CanExecute;
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null? true : _canExecute();
        }
    }
}
