using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Cadmus.Foundation;
using Cadmus.ParameterEditor.Framework;
using Cadmus.Parametrizer;
using Cadmus.Parametrizer.Options;
using Cadmus.VisualFoundation.Framework;

namespace Cadmus.ParameterEditor.ViewModels
{
    public class ParameterViewModel : ViewModelBase
    {
        public Parameter Parameter { get; protected set; }

        public string Title => Parameter?.Title ?? Parameter?.Name;

        public string Description => Parameter?.Description;

        public EditorOptions Editor
        {
            get
            {
                if (Parameter == null || Parameter?.Editor == EditorOptions.NotSet)
                    return EditorOptions.Text;
                return Parameter.Editor;
            }
        }

        public bool IsVisible => Parameter?.Editor != EditorOptions.Hidden;

        public bool IsEncrypted => Parameter?.Encryption == EncryptionOptions.Yes;

        public bool IsEncryptable => Parameter?.Encryptable == EncryptableOptions.Yes;

        public bool IsReadOnly => IsEncrypted;

        public string Value
        {
            get
            {
                if (IsEncrypted)
                    return "***** encrypted *****";
                return Parameter?.Value;
            }
            set
            {
                Parameter.Value = value;
                OnPropertyChanged();
            }
        }

        public Caliburn.Micro.BindableCollection<CommandViewModel> Commands { get; protected set; }

        public ParameterViewModel(Parameter parameter)
        {
            Parameter = parameter;
            Commands = new Caliburn.Micro.BindableCollection<CommandViewModel>();
            Commands.Add(new CommandViewModel(new ActionCommand(Encrypt)) {Title = "Encrypt"});
            Commands.Add(new CommandViewModel(new ActionCommand(PickFolder)) {Title = "Pick" });
            //Commands.Add(new CommandViewModel(new ActionCommand(GenerateMachineValidationKey)) {Title = "GenerateMachineValidationKey" });
        }

        public DataTemplate GetTemplate()
        {
            var resourceName = Editor + "ParamEditorTemplate";
            return (DataTemplate)App.Current.Resources[resourceName];
        }

        public void Encrypt()
        {
            Parameter.Encryption = EncryptionOptions.Yes;
            OnPropertyChanged(nameof(IsEncrypted));
            OnPropertyChanged(nameof(IsReadOnly));
            OnPropertyChanged(nameof(Value));
        }

        public void Decrypt()
        {
            Parameter.Encryption = EncryptionOptions.NotSet;
            OnPropertyChanged(nameof(IsEncrypted));
            OnPropertyChanged(nameof(IsReadOnly));
        }

        public void ClearEncrypted()
        {
            Parameter.Value = null;
            Parameter.Encryption = EncryptionOptions.NotSet;
            OnPropertyChanged(nameof(IsEncrypted));
            OnPropertyChanged(nameof(Value));
        }

        public void PickFolder()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Value = dialog.SelectedPath;
            }
        }

        public void GenerateGuid()
        {
            var guid = Guid.NewGuid();
            this.Value = guid.ToString("B");
        }

        public void GenerateMachineValidationKey()
        {
            var gen = new MachineKeyGenerator();
            this.Value = gen.GenerateValidationKey();
        }

        public void GenerateMachineDecryptionKey()
        {
            var gen = new MachineKeyGenerator();
            this.Value = gen.GenerateDecryptionKey();
        }
    }
}
