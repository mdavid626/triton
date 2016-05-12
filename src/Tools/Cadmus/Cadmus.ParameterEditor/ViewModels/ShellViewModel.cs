using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Cadmus.Foundation;
using Cadmus.ParameterEditor.Framework;
using Cadmus.ParameterEditor.Interfaces;
using Cadmus.ParameterEditor.Properties;

namespace Cadmus.ParameterEditor.ViewModels
{
    public class ShellViewModel : ViewModelBase, IShell
    {
        public ShellViewModel()
        {
            InstallFolder = AppDomain.CurrentDomain.BaseDirectory;
            Title = CreateTitle();
            AddLogger(new FileLogger("deploy.log"));
            Task.Delay(100).ContinueWith(t => Load());
        }

        public string Title { get; protected set; }

        public string InstallFolder { get; protected set; }

        private string _deployedAppVersion;
        public string DeployedAppVersion
        {
            get { return _deployedAppVersion; }
            set
            {
                _deployedAppVersion = value;
                OnPropertyChanged();
            }
        }

        private ViewModelBase _currentScreen;
        public ViewModelBase CurrentScreen
        {
            get { return _currentScreen; }
            set
            {
                _currentScreen = value;
                OnPropertyChanged();
            }
        }

        private ILogger _logger = new MultiLogger();
        public ILogger Logger
        {
            get { return _logger; }
            set
            {
                _logger = value;
                AppBootstrapper.Current.Logger = value;
            }
        }

        private ILogger _flowDocumentLogger;
        public ILogger FlowDocumentLogger
        {
            get { return _flowDocumentLogger; }
            set
            {
                _flowDocumentLogger = value;
                AddLogger(_flowDocumentLogger);
            }
        }

        public void AddLogger(ILogger logger)
        {
            ((MultiLogger)Logger)?.Add(logger);
        }

        public ConfiguratorViewModel ConfiguratorViewModel { get; protected set; }

        public void Load()
        {
            DeployedAppVersion = LoadDeployedAppVersion("VERSION");
            CurrentScreen = ConfiguratorViewModel = new ConfiguratorViewModel(this);
            ConfiguratorViewModel.PropertyChanged += ConfiguratorViewModelOnPropertyChanged;
            ConfiguratorViewModel.OpenDefaultConfig();
        }

        private string LoadDeployedAppVersion(string path)
        {
            if (System.IO.File.Exists(path))
            {
                return System.IO.File.ReadLines(path).FirstOrDefault() ?? "0.0.0.0";
            }
            return "0.0.0.0";
        }

        private void ConfiguratorViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.IsIn(nameof(CanSaveConfig), nameof(CanCloseConfig), nameof(CanNewConfig),
                nameof(CanReloadConfig)))
            {
                OnPropertyChanged(e.PropertyName);
            }
        }

        private string CreateTitle()
        {
            var settings = new Settings();
            return settings.WindowTitle;
        }

        public bool CanNewConfig => ConfiguratorViewModel?.CanNewConfig ?? false;

        public void NewConfig()
        {
            ConfiguratorViewModel?.NewConfig();
        }

        public void NewConfigGuarded()
        {
            if (CanNewConfig)
                NewConfig();
        }

        public void OpenConfig()
        {
            ConfiguratorViewModel?.OpenConfig();
        }

        public bool CanSaveConfig => ConfiguratorViewModel?.CanSaveConfig ?? false;

        public void SaveConfig()
        {
            ConfiguratorViewModel?.SaveConfig();
        }

        public void SaveConfigGuarded()
        {
            if (CanSaveConfig)
                SaveConfig();
        }

        public bool CanReloadConfig => ConfiguratorViewModel?.CanReloadConfig ?? false;

        public void ReloadConfig()
        {
            ConfiguratorViewModel?.ReloadConfig();
        }

        public void ReloadConfigGuarded()
        {
            if (CanReloadConfig)
                ReloadConfig();
        }

        public bool CanCloseConfig => ConfiguratorViewModel?.CanCloseConfig ?? false;

        public void CloseConfig()
        {
            ConfiguratorViewModel?.CloseConfig();
        }

        public void CloseConfigGuarded()
        {
            if (CanCloseConfig)
                CloseConfig();
        }

        public void Exit()
        {
            System.Windows.Application.Current.Shutdown();
        }

        public void ClearLog()
        {
            Logger.Clear();
        }

        public void OpenCommandPrompt()
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    WorkingDirectory = InstallFolder
                }
            };
            process.Start();
        }

        public void OpenInstallFolder()
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "explorer.exe",
                    Arguments = InstallFolder,
                }
            };
            process.Start();
        }

        public void ShowAbout()
        {
            var version = new Cadmus.Foundation.AppVersionInfo();
            var msg = $"{Title}\nParametrizer version: {version.GetCurrentVersion()}";
            System.Windows.MessageBox.Show(msg, Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
