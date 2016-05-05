using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cadmus.ParameterEditor.Framework;
using Cadmus.VisualFoundation.Framework;

namespace Cadmus.ParameterEditor.ViewModels
{
    public class ConnectionStringEditorViewModel : ViewModelBase, IDialogViewModel
    {
        public string ConnectionString { get; protected set; }

        public bool? DialogResult { get; protected set; }

        public Action Close { get; set; }

        private string _dataSource;
        public string DataSource
        {
            get { return _dataSource; }
            set
            {
                _dataSource = value;
                OnPropertyChanged();
            }
        }

        private string _initialCatalog;
        public string InitialCatalog
        {
            get { return _initialCatalog; }
            set
            {
                _initialCatalog = value;
                OnPropertyChanged();
            }
        }

        private bool _isIntegratedSecurity;
        public bool IsIntegratedSecurity
        {
            get { return _isIntegratedSecurity; }
            set
            {
                _isIntegratedSecurity = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNotIntegratedSecurity));
            }
        }

        public bool IsNotIntegratedSecurity => !IsIntegratedSecurity;

        private string _userId;
        public string UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public ConnectionStringEditorViewModel(string connectionString)
        {
            ConnectionString = connectionString;
            Load();
        }

        public void Ok()
        {
            Save();
            DialogResult = true;
            Close();
        }

        public void Cancel()
        {
            Close();
        }

        private void Load()
        {
            var sqlBuilder = new SqlConnectionStringBuilder(ConnectionString);
            DataSource = sqlBuilder.DataSource;
            InitialCatalog = sqlBuilder.InitialCatalog;
            IsIntegratedSecurity = sqlBuilder.IntegratedSecurity;
            UserId = sqlBuilder.UserID;
            Password = sqlBuilder.Password;
        }

        private void Save()
        {
            var sqlBuilder = new SqlConnectionStringBuilder(ConnectionString);
            sqlBuilder.DataSource = DataSource;
            sqlBuilder.InitialCatalog = InitialCatalog;
            sqlBuilder.IntegratedSecurity = IsIntegratedSecurity;
            if (!IsIntegratedSecurity)
            {
                sqlBuilder.UserID = UserId;
                sqlBuilder.Password = Password;
            }
            else
            {
                sqlBuilder.Remove("User Id");
                sqlBuilder.Remove("Password");
            }
            ConnectionString = sqlBuilder.ConnectionString;
        }
    }
}
