using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadmus.VisualFoundation.Framework;
using System;

namespace Cadmus.Foundation
{
    public class ActionCommand : NotifyPropertyChangedBase, ICommand
    {
        public Action Action { get; protected set; }

        public ActionCommand(Action action)
        {
            Action = action;
        }

        public bool CanExecute
        {
            get
            {
                return true;
            }
        }

        public void Execute()
        {
            Action();
        }
    }
}
