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

        private string _computerName;
        public string ComputerName
        {
            get { return _computerName; }
            set
            {
                _computerName = value;
                OnPropertyChanged();
            }
        }

        public Caliburn.Micro.BindableCollection<string> ComputerNames { get; protected set; }

        public Caliburn.Micro.BindableCollection<LookupItem> ScopeItems { get; protected set; }

        public Caliburn.Micro.BindableCollection<LookupItem> ModeItems { get; protected set; }

        public bool IsManualMode => ModeOptions.Manual.Equals(SelectedMode.Value);

        public bool IsAutoMode => ModeOptions.Auto.Equals(SelectedMode.Value);

        public EncryptionViewModel(string value, List<string> computerNames = null)
        {
            OriginalValue = value;
            InitScopeItems();
            InitModeItems();
            InitComputerNames(computerNames);
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

        private void InitComputerNames(List<string> computerNames)
        {
            ComputerNames = new Caliburn.Micro.BindableCollection<string>();
            if (computerNames != null)
                ComputerNames.AddRange(computerNames);
            //ComputerName = ComputerNames.FirstOrDefault();
        }

        public bool CanOk => IsManualMode && !EncryptedValue.IsNullOrEmpty() || 
                             IsAutoMode && !OriginalValue.IsNullOrEmpty();

        public void Ok()
        {
            if (Encrypt())
            {
                DialogResult = true;
                Close();
            }
        }

        public void Cancel()
        {
            Close();
        }

        private bool Encrypt()
        {
            if (IsAutoMode)
            {
                if (ComputerName.IsNullOrEmpty())
                {
                    var protector = new PasswordProtector();
                    EncryptedValue = protector.Protect(OriginalValue, PasswordProtectionScope.LocalMachine);
                    return true;
                }
                else
                {
                    //var factory = new ImpersonatorFactory()
                    //{
                    //    IsEnabled = true,
                    //    Username = "testuser",
                    //    Password = "heslo",
                    //    Domain = "LILI"
                    //};
                    //using (factory.Create())
                    {
                        var powershell = new PowerShellExecutor(ComputerName);
                        if (powershell.Execute())
                        {
                            EncryptedValue = powershell.Result;
                            return true;
                        }
                    }
                }
                return false;
            }
            return true;
        }
    }
}
