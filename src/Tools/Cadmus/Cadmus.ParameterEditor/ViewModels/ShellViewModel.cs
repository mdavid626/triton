using System;
using System.Collections.Generic;
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
            AppVersion = "1.0.0.0";
            Task.Delay(100).ContinueWith(t => Load());
        }

        public string Title { get; protected set; }

        public string InstallFolder { get; protected set; }

        public string AppVersion { get; protected set; }

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

        private ILogger _logger = new DefaultLogger();
        public ILogger Logger
        {
            get { return _logger; }
            set
            {
                _logger = value;
                AppBootstrapper.Current.Logger = value;
            }
        }

        public ConfiguratorViewModel ConfiguratorViewModel { get; protected set; }

        public void Load()
        {
            CurrentScreen = ConfiguratorViewModel = new ConfiguratorViewModel(this);
            ConfiguratorViewModel.OpenDefaultConfig();
        }

        private string CreateTitle()
        {
            var settings = new Settings();
            return settings.WindowTitle;
        }

        public void NewConfig()
        {
            ConfiguratorViewModel?.NewConfig();
        }

        public void OpenConfig()
        {
            ConfiguratorViewModel?.OpenConfig();
        }

        public void SaveConfig()
        {
            ConfiguratorViewModel?.SaveConfig();
        }

        public void ReloadConfig()
        {
            ConfiguratorViewModel?.ReloadConfig();
        }

        public void CloseConfig()
        {
            ConfiguratorViewModel?.CloseConfig();
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
