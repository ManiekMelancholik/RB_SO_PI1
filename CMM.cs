using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RB_SO_PI1
{
    class CMM : ICommand
    {


        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;
        public CMM(Action<object> ex, Func<object, bool> cex)
        {
            execute = ex;
            canExecute = cex;


        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }


        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
            {
                return false;
            }


            return canExecute.Invoke(parameter);

        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                execute(parameter);
            }
        }
    }

}
