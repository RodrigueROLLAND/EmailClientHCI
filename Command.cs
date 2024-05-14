using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

//Copy of LabWork

namespace EmailClient
{
    class Command : ICommand   //pour implémenter l'event et CanExecute et Execute il faut cliquer sur Icommand et faire ctrl + point et après on fait le premier truc (qui permet de rajouter le using) et après on implémente
    {

        public Command(Action<object?> action) 
        {
            this.action = action;
        }
        public event EventHandler? CanExecuteChanged;


        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (action != null)
            {
                action(parameter);
            }

        }

        private Action<object?> action;
    }
}
