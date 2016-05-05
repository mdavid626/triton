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
        public bool? DialogResult { get; protected set; }

        public Action Close { get; set; }

        public string EncryptedValue { get; protected set; }

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

        public Caliburn.Micro.BindableCollection<LookupItem> ScopeItems { get; protected set; }

        public EncryptionViewModel(string value)
        {
            OriginalValue = value;
            InitScopeItems();
        }

        private void InitScopeItems()
        {
            ScopeItems = new Caliburn.Micro.BindableCollection<LookupItem>();
            ScopeItems.Add(new LookupItem() { Text = "Machine", Value = PasswordProtectionScope.LocalMachine });
            ScopeItems.Add(new LookupItem() { Text = "User", Value = PasswordProtectionScope.CurrentUser });
            SelectedScope = ScopeItems.FirstOrDefault();
        }

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
            var protector = new PasswordProtector();
            EncryptedValue = protector.Protect(OriginalValue, PasswordProtectionScope.LocalMachine);
        }
    }
}
