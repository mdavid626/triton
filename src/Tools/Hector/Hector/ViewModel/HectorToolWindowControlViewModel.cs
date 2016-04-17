using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Hector.Annotations;
using Hector.Framework;

namespace Hector.ViewModel
{
    public class HectorToolWindowControlViewModel : INotifyPropertyChanged
    {
        private readonly DbVersionCreator _dbVersionCreator;
        private readonly DbUpManager _dbUpManager;

        public HectorToolWindowControlViewModel()
        {
            // TODO injection
            _dbVersionCreator = new DbVersionCreator();
            _dbUpManager = new DbUpManager();
        }

        private string _dbVersionDescription;
        public string DbVersionDescription
        {
            get { return _dbVersionDescription; }
            set
            {
                _dbVersionDescription = value;
                OnPropertyChanged();
            }
        }

        private string _dbServer;
        public string DbServer
        {
            get { return _dbServer; }
            set
            {
                _dbServer = value;
                OnPropertyChanged();
            }
        }

        private string _dbName;
        public string DbName
        {
            get { return _dbName; }
            set
            {
                _dbName = value;
                OnPropertyChanged();
            }
        }

        public void CreateDbVersion()
        {
            _dbVersionCreator.Create(DbVersionDescription);
            DbVersionDescription = null;
        }

        public void UpgradeDb()
        {
            _dbUpManager.Upgrade(DbServer, DbName);
        }

        public void CreateDb()
        {
            _dbUpManager.Create(DbServer, DbName);
        }

        public void DropDb()
        {
            _dbUpManager.Drop(DbServer, DbName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
