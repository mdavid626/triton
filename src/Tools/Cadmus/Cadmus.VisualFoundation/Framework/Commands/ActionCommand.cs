using System;
using Cadmus.Foundation;

namespace Cadmus.VisualFoundation.Framework.Commands
{
    public class ActionCommand : ICommand
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
