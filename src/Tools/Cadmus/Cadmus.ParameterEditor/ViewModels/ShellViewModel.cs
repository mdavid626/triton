using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Cadmus.ParameterEditor.Framework;
using Cadmus.ParameterEditor.Interfaces;
using Cadmus.ParameterEditor.Properties;

namespace Cadmus.ParameterEditor.ViewModels
{
    public class ShellViewModel : ViewModelBase, IShell
    {
        public ShellViewModel()
        {
            Title = CreateTitle();
            InstallFolder = AppDomain.CurrentDomain.BaseDirectory;
            AppVersion = "1.0.0.0";
        }

        public string Title { get; protected set; }

        public string InstallFolder { get; protected set; }

        public string AppVersion { get; protected set; }

        private string CreateTitle()
        {
            var settings = new Settings();
            return settings.WindowTitle;
        }

        public void NewConfig()
        {
            
        }

        public void OpenConfig()
        {

        }

        public void SaveConfig()
        {

        }

        public void ReloadConfig()
        {

        }

        public void Exit()
        {
            Application.Current.Shutdown();
        }

        public void ClearLog()
        {

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
