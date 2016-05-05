using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadmus.Foundation;
using Cadmus.ParameterEditor.Framework;
using Cadmus.VisualFoundation.Framework;

namespace Cadmus.ParameterEditor.ViewModels
{
    public class EncryptionViewModel : ViewModelBase, IDialogViewModel
    {
        public enum ModeOptions
        {
            Auto,
            Manual
        }

        public bool? DialogResult { get; protected set; }

        public Action Close { get; set; }

        private string _encryptedValue;
        public string EncryptedValue
        {
            get { return _encryptedValue; }
            set
            {
                _encryptedValue = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanOk));
            }
        }

        public string OriginalValue { get; protected set; }

        private LookupItem _selectedScope;

        public LookupItem SelectedScope
        {
            get { return _selectedScope; }
            set
            {
                _selectedScope = value;
                OnPropertyChanged();
            }
        }

        private LookupItem _selectedMode;

        public LookupItem SelectedMode
        {
            get { return _selectedMode; }
            set
            {
                _selectedMode = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsManualMode));
                OnPropertyChanged(nameof(IsAutoMode));
                OnPropertyChanged(nameof(CanOk));
            }
        }

        public Caliburn.Micro.BindableCollection<LookupItem> ScopeItems { get; protected set; }

        public Caliburn.Micro.BindableCollection<LookupItem> ModeItems { get; protected set; }

        public bool IsManualMode => ModeOptions.Manual.Equals(SelectedMode.Value);

        public bool IsAutoMode => ModeOptions.Auto.Equals(SelectedMode.Value);

        public EncryptionViewModel(string value)
        {
            OriginalValue = value;
            InitScopeItems();
            InitModeItems();
        }

        private void InitScopeItems()
        {
            ScopeItems = new Caliburn.Micro.BindableCollection<LookupItem>();
            ScopeItems.Add(new LookupItem() { Text = "Machine", Value = PasswordProtectionScope.LocalMachine });
            ScopeItems.Add(new LookupItem() { Text = "User", Value = PasswordProtectionScope.CurrentUser });
            SelectedScope = ScopeItems.FirstOrDefault();
        }

        private void InitModeItems()
        {
            ModeItems = new Caliburn.Micro.BindableCollection<LookupItem>();
            ModeItems.Add(new LookupItem() { Text = "Auto", Value = ModeOptions.Auto });
            ModeItems.Add(new LookupItem() { Text = "Manual", Value = ModeOptions.Manual });
            SelectedMode = OriginalValue.IsNullOrEmpty() ? ModeItems.Last() : ModeItems.First();
        }

        public bool CanOk => IsManualMode && !EncryptedValue.IsNullOrEmpty() || 
                             IsAutoMode && !OriginalValue.IsNullOrEmpty();

        public void Ok()
        {
            Encrypt();
            DialogResult = true;
            Close();
        }

        public void Cancel()
        {
            Close();
        }

        private void Encrypt()
        {
            if (IsAutoMode)
            {
                var protector = new PasswordProtector();
                EncryptedValue = protector.Protect(OriginalValue, PasswordProtectionScope.LocalMachine);
            }
        }
    }
}
