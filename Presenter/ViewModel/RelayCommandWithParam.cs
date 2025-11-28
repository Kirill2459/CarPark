using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Presenter.ViewModel
{
    public class RelayCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private event Action<T> _execute1;
        private event Action<T, T> _execute2;
        private event Func<bool> _canExecute;

        public RelayCommand(Action<T> Execute)
        {
            _execute1 = Execute;
        }

        public RelayCommand(Action<T, T> Execute)
        {
            _execute2 = Execute;
        }

        public RelayCommand(Action<T> Execute, Func<bool> CanExecute)
        {
            _execute1 = Execute;
            _canExecute = CanExecute;
        }

        public RelayCommand(Action<T, T> Execute, Func<bool> CanExecute)
        {
            _execute2 = Execute;
            _canExecute = CanExecute;
        }

        public void Execute(object parameter)
        {
            //_execute();
            if (_execute1 != null && parameter is T param)
            {
                _execute1((T)parameter);
            }
            else if (_execute2 != null && parameter is Tuple<T, T> tuple)
            {
                _execute2(tuple.Item1, tuple.Item2);
            }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute();
        }
    }
}
