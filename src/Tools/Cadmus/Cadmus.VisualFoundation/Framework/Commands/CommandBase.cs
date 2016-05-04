using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadmus.Foundation;

namespace Cadmus.VisualFoundation.Framework.Commands
{
    public class CommandBase : NotifyPropertyChangedBase, ICommand
    {
        public virtual bool CanExecute
        {
            get { return true; }
        }

        public virtual void Execute()
        {
            
        }
    }
}
