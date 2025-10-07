using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace Mph.WPFAppPlugin
{
    public abstract class PluginApp :Application
    {
        public PluginApp() : base()
        {

        }

        public PluginAppConfig GlobalAppConfig { get; private set; }

        public PluginRuntime Runtime { get; private set; }

        internal void PushException(Exception exception)
        {
            OnExceptionHandle(exception);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _appStartArgs = e.Args ?? [];
            Initialize();
        }

        protected abstract void OnAppStartUp(PluginAppConfig config);

        protected virtual void OnExceptionHandle(object exceptionObject)
        {

        }

        private string[] _appStartArgs;

        private void Initialize()
        {
            // 初始化配置
            GlobalAppConfig = new PluginAppConfig
            {
                StartArgs = _appStartArgs
            };
            OnAppStartUp(GlobalAppConfig);

            // 初始化运行时
            Runtime = new PluginRuntime(this);
            Runtime.LoadPreDefinedPluginsFromConfig(GlobalAppConfig);
            Runtime.Start();

            // 注册异常处理
            ExceptionRegister();
        }

        private void ExceptionRegister()
        {
            DispatcherUnhandledException += Current_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string message = e.ExceptionObject is Exception ex ? ex.Message : e.ExceptionObject.ToString();
            OnExceptionHandle(e.ExceptionObject);
            if (!GlobalAppConfig.NeedShowDefaultDialog)
            {
                return;
            }
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            OnExceptionHandle(e.Exception);
            if (!GlobalAppConfig.NeedShowDefaultDialog)
            {
                return;
            }
            MessageBox.Show(e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            OnExceptionHandle(e.Exception);
            if (!GlobalAppConfig.NeedShowDefaultDialog)
            {
                return;
            }
            MessageBox.Show(e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
