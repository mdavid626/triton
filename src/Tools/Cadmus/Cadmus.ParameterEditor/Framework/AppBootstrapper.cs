using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cadmus.Foundation;
using Cadmus.ParameterEditor.Interfaces;
using Cadmus.ParameterEditor.ViewModels;
using Cadmus.VisualFoundation.Framework;
using Caliburn.Micro;

namespace Cadmus.ParameterEditor.Framework
{
    public class AppBootstrapper : BootstrapperBase
    {
        private SimpleContainer _container;

        public ILogger Logger { get; set; } = new MessageBoxLogger();

        public static AppBootstrapper Current { get; protected set; }

        public AppBootstrapper()
        {
            Initialize();
            Current = this;
        }

        protected override void Configure()
        {
            _container = new SimpleContainer();

            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<IEventAggregator, EventAggregator>();
            _container.Singleton<IShell, ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            var instance = _container.GetInstance(service, key);
            if (instance != null)
                return instance;

            throw new InvalidOperationException("Could not locate any instances.");
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            App.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            DisplayRootViewFor<IShell>();
        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            Logger.LogError(e.Exception?.ToString());
        }
    }
}
