using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadmus.VisualFoundation.Framework.Commands
{
    public class DialogCommand : DialogCommand<IDialogViewModel>
    {
        public DialogCommand(object viewModel) : base((IDialogViewModel)viewModel)
        {

        }

        public static bool? ShowDialog<TViewModel>(TViewModel vm) where TViewModel : IDialogViewModel
        {
            var cmd = new DialogCommand<TViewModel>(vm);
            cmd.Execute();
            return cmd.Result;
        }
    }

    public class DialogCommand<TViewModel> : CommandBase<bool?> where TViewModel : IDialogViewModel
    {
        public TViewModel ViewModel { get; protected set; }

        public DialogCommand(TViewModel viewModel)
        {
            ViewModel = viewModel;
        }
        
        public override void Execute()
        {
            var view = Caliburn.Micro.ViewLocator.LocateForModel(ViewModel, null, null);
            Caliburn.Micro.ViewModelBinder.Bind(ViewModel, view, null);
            ViewModel.Close = ((IDialogView) view).Close;
            ((IDialogView)view)?.ShowDialog();
            Result = ViewModel.DialogResult;
        }
    }
}
