using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadmus.Foundation;

namespace Cadmus.VisualFoundation.Framework
{
    public class CommandViewModel : NotifyPropertyChangedBase
    {
        public string Title { get; set; }

        public ICommand Command { get; protected set; }

        public bool IsVisible => true;

        public bool CanExecute => true;

        public CommandViewModel(ICommand command)
        {
            Command = command;
        }

        public void Execute()
        {
            Command.Execute();
        }
    }
}
