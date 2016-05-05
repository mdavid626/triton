using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadmus.Foundation;

namespace Cadmus.VisualFoundation.Framework.Commands
{
    public class CommandBase : CommandBase<object>
    {
        
    }

    public class CommandBase<TResult> : NotifyPropertyChangedBase, IGuiCommand
    {
        private bool _canExecute = true;
        public virtual bool CanExecute
        {
            get { return _canExecute; }
            set
            {
                _canExecute = value;
                OnPropertyChanged();
            }
        }

        private string _title;
        public virtual string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }
        
        public TResult Result { get; protected set; }

        public virtual void Execute()
        {
            
        }
    }
}
