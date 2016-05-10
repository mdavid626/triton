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
using Cadmus.Foundation.Metadata;
using Cadmus.VisualFoundation.Framework.Commands;

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

        public bool IsVisible
        {
            get
            {
                return (Parameter.Steps.Contains(Parent?.SelectedStep?.Name) || 
                        !Parameter.Steps.Any()) && 
                       Parameter?.Editor != EditorOptions.Hidden;
            }
        } 

        public bool IsEncrypted => Parameter?.Encrypted == EncryptionOptions.Yes;

        public bool IsEncryptable => Parameter?.Encryptable == EncryptableOptions.Yes;

        public bool IsReadOnly => IsEncrypted;

        public string Value
        {
            get
            {
                if (IsEncrypted)
                    return "*****encrypted*****";
                return Parameter?.Value;
            }
            set
            {
                Parameter.Value = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanEncrypt));
            }
        }

        public Caliburn.Micro.BindableCollection<CommandViewModel> Commands { get; protected set; }

        public Caliburn.Micro.BindableCollection<LookupItem> Lookups { get; protected set; }

        public ConfiguratorViewModel Parent { get; protected set; }

        public ParameterViewModel(Parameter parameter, ConfiguratorViewModel parent)
        {
            Parameter = parameter;
            Parent = parent;
            Parent.PropertyChanged += Parent_PropertyChanged;
            CreateCommands();
            CreateLookups();
        }

        public void DealloateParent()
        {
            Parent.PropertyChanged -= Parent_PropertyChanged;
        }

        private void Parent_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Parent.SelectedStep))
            {
                OnPropertyChanged(nameof(IsVisible));
            }
        }

        private void CreateCommands()
        {
            Commands = CreateCoroutineOperations();
        }

        private void CreateLookups()
        {
            Lookups = new Caliburn.Micro.BindableCollection<LookupItem>();
            if (Editor == EditorOptions.TrueFalse)
            {
                Lookups.Add(new LookupItem() {Text = "True", Value = "True"});
                Lookups.Add(new LookupItem() {Text = "False", Value = "False" });
            } 
            else if (Editor == EditorOptions.Lookup)
            {
                Lookups.AddRange(Parameter.Lookups.Select(item => new LookupItem() { Text = item.Text, Value = item.Value}));
            }
        }

        public DataTemplate GetTemplate()
        {
            var name = Editor;
            if (Editor == EditorOptions.TrueFalse)
                name = EditorOptions.Lookup;
            var resourceName = name + "ParamEditorTemplate";
            var resources = App.Current.Resources;
            var template = resources[resourceName];
            if (template == null)
                template = resources["TextParamEditorTemplate"];
            return (DataTemplate)template;
        }

        private void RefreshEncryption()
        {
            OnPropertyChanged(nameof(IsEncrypted));
            OnPropertyChanged(nameof(IsReadOnly));
            OnPropertyChanged(nameof(Value));
            OnPropertyChanged(nameof(CanEncrypt));
            //OnPropertyChanged(nameof(CanDecrypt));
            OnPropertyChanged(nameof(CanClearEncrypted));
        }

        public bool CanEncrypt => IsEncryptable && !IsEncrypted /*&& !Value.IsNullOrEmpty()*/;

        [Operation(Title = "Encrypt")]
        public void Encrypt()
        {
            var computerNames = Parent.ParameterViewModels.Where(p => p.Editor == EditorOptions.ComputerName)
                                                          .Select(p => p.Value)
                                                          .Where(v => !v.IsNullOrEmpty())
                                                          .Distinct()
                                                          .ToList();
            var vm = new EncryptionViewModel(Value, computerNames);
            var result = DialogCommand.ShowDialog(vm);
            if (result == true)
            {
                Value = vm.EncryptedValue;
                Parameter.Encrypted = EncryptionOptions.Yes;
                RefreshEncryption();
            }
        }

        //public bool CanDecrypt => IsEncrypted;

        //[Operation(Title = "Decrypt")]
        //public void Decrypt()
        //{
        //    Parameter.Encrypted = EncryptionOptions.NotSet;
        //    RefreshEncryption();
        //}

        public bool CanClearEncrypted => IsEncrypted;

        [Operation(Title = "Clear", Description = "Clear encrypted value")]
        public void ClearEncrypted()
        {
            Parameter.Value = null;
            Parameter.Encrypted = EncryptionOptions.NotSet;
            RefreshEncryption();
        }

        public bool CanPickFolder => Editor == EditorOptions.FolderPicker;

        [Operation(Title = "Pick", Description = "Pick folder")]
        public void PickFolder()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Value = dialog.SelectedPath;
            }
        }

        public bool CanGenerateGuid => Editor == EditorOptions.GuidGenerator;

        [Operation(Title = "Generate", Description = "Genereate new guid")]
        public void GenerateGuid()
        {
            var guid = Guid.NewGuid();
            Value = guid.ToString("B");
        }

        public bool CanGenerateMachineValidationKey => Editor == EditorOptions.MachineValidationKey;

        [Operation(Title = "Generate", Description = "Generate new validation key")]
        public void GenerateMachineValidationKey()
        {
            var gen = new MachineKeyGenerator();
            Value = gen.GenerateValidationKey();
        }

        public bool CanGenerateMachineDecryptionKey => Editor == EditorOptions.MachineDecryptionKey;

        [Operation(Title = "Generate", Description = "Generate new decryption key")]
        public void GenerateMachineDecryptionKey()
        {
            var gen = new MachineKeyGenerator();
            Value = gen.GenerateDecryptionKey();
        }

        public bool CanEditConnString => Editor == EditorOptions.ConnectionString;

        [Operation(Title = "Edit", Description = "Edit connection string")]
        public void EditConnString()
        {
            var vm = new ConnectionStringEditorViewModel(Value);
            var result = DialogCommand.ShowDialog(vm);
            if (result == true)
            {
                Value = vm.ConnectionString;
            }
        }
    }
}
