using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cadmus.Foundation;
using Cadmus.ParameterEditor.Framework;
using Cadmus.ParameterEditor.Interfaces;
using Cadmus.Parametrizer;
using Cadmus.VisualFoundation.Framework;

namespace Cadmus.ParameterEditor.ViewModels
{
    public class ConfiguratorViewModel : ViewModelBase
    {
        private readonly IShell _shell;
        
        public Caliburn.Micro.BindableCollection<ParameterViewModel> ParameterViewModels { get; }

        public Caliburn.Micro.BindableCollection<Step> Steps { get; }

        public Caliburn.Micro.BindableCollection<CommandViewModel> CurrentOperations { get; }

        public ILogger Logger => _shell.Logger;

        private ConfigManager _configManager;
        public ConfigManager ConfigManager
        {
            get { return _configManager; }
            set
            {
                _configManager = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanSaveConfig));
                OnPropertyChanged(nameof(CanReloadConfig));
                OnPropertyChanged(nameof(CanNewConfig));
                OnPropertyChanged(nameof(CanCloseConfig));
            }
        }

        private Step _selectedStep;

        public Step SelectedStep
        {
            get { return _selectedStep; }
            set
            {
                _selectedStep = value;
                LoadCurrentOperations();
                OnPropertyChanged();
            }
        }

        private void LoadCurrentOperations()
        {
            CurrentOperations.Clear();
            if (SelectedStep != null)
            {
                CurrentOperations.AddRange(
                    SelectedStep.Operations.Select(o => new CommandViewModel(o.ToCommand(Logger, ConfigManager))
                    {
                        Title = o.Title,
                        Description = o.Description
                    }));
            }
        }

        public ConfiguratorViewModel(IShell shell)
        {
            _shell = shell;
            ParameterViewModels = new Caliburn.Micro.BindableCollection<ParameterViewModel>();
            Steps = new Caliburn.Micro.BindableCollection<Step>();
            CurrentOperations = new Caliburn.Micro.BindableCollection<CommandViewModel>();
        }

        public bool CanNewConfig => ConfigManager != null;

        public void NewConfig()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Config files (*.xml)|*.xml";
            var result = saveFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                ConfigManager.CreateAndSaveEmptyConfig(saveFileDialog.FileName);
                OpenConfig(saveFileDialog.FileName);
            }
        }

        public void OpenConfig()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Config files (*.xml)|*.xml|All files (*.*)|*.*";
            var result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                OpenConfig(openFileDialog.FileName);
            }
        }

        public void OpenConfig(string path)
        {
            var man = new ConfigManager(path);
            man.Load();
            ConfigManager = man;

            ParameterViewModels.Apply(p => p.DealloateParent());
            ParameterViewModels.Clear();
            ParameterViewModels.AddRange(man.Config.Parameters.Select(p => new ParameterViewModel(p, this)));

            var currentStep = SelectedStep;
            Steps.Clear();
            Steps.AddRange(man.Config.Steps);
            SelectedStep = Steps.FirstOrDefault(s => s.Name == currentStep?.Name) 
                                ?? Steps.FirstOrDefault(s => s.IsDefault) 
                                ?? Steps.FirstOrDefault();

            Logger.LogSuccess("Config loaded: " + path);
        }

        public void OpenDefaultConfig()
        {
            OpenConfig(GetDefaultConfigPath());
        }

        private string GetDefaultConfigPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.xml");
        }

        public bool CanSaveConfig => ConfigManager != null;

        public void SaveConfig()
        {
            ConfigManager?.Save();
            Logger.LogSuccess("Config saved: " + ConfigManager?.ConfigPath);
        }

        public bool CanReloadConfig => ConfigManager != null;

        public void ReloadConfig()
        {
            OpenConfig(_configManager.ConfigPath);
        }

        public bool CanCloseConfig => ConfigManager != null;

        public void CloseConfig()
        {
            ConfigManager = null;
            ParameterViewModels.Clear();
            Steps.Clear();
        }
    }
}
