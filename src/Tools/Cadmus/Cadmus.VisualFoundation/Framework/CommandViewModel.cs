using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadmus.Foundation;

namespace Cadmus.VisualFoundation.Framework
{
    public class CommandViewModel : NotifyPropertyChangedBase
    {
        private string _title;

        public string Title
        {
            get { return (Command as IGuiCommand)?.Title ?? _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public string Description { get; set; }

        public ICommand Command { get; protected set; }

        public bool IsVisible => CanExecute;

        public bool CanExecute => Command.CanExecute;

        public CommandViewModel(ICommand command)
        {
            Command = command;
            var notify = Command as INotifyPropertyChanged;
            if (notify != null)
                notify.PropertyChanged += Command_PropertyChanged;
        }

        private void Command_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Command.CanExecute))
            {
                OnPropertyChanged(nameof(CanExecute));
                OnPropertyChanged(nameof(IsVisible));
            }
            if (e.PropertyName == nameof(Title))
            {
                OnPropertyChanged(e.PropertyName);
            }
        }

        public void Execute()
        {
            Command.Execute();
        }
    }
}
