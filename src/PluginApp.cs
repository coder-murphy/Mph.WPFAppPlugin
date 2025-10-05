using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Mph.WPFAppPlugin
{
    public class PluginApp :Application
    {
        public PluginApp() : base()
        {

        }

        public PluginAppStartingConfig GlobalAppConfig { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            OnAppStartUp(new PluginAppStartingConfig());
            DispatcherUnhandledException += Current_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        protected virtual void OnAppStartUp(PluginAppStartingConfig config)
        {
            GlobalAppConfig = config;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string message = e.ExceptionObject is Exception ex ? ex.Message : e.ExceptionObject.ToString();
            if(!GlobalAppConfig.NeedShowDefaultDialog)
            {
                return;
            }
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            if (!GlobalAppConfig.NeedShowDefaultDialog)
            {
                return;
            }
            MessageBox.Show(e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (!GlobalAppConfig.NeedShowDefaultDialog)
            {
                return;
            }
            MessageBox.Show(e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
