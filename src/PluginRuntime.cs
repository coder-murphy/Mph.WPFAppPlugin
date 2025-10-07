using Mph.WPFAppPlugin.Interfaces;
using Mph.WPFAppPlugin.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Mph.WPFAppPlugin
{
    public class PluginRuntime
    {
        internal PluginRuntime(PluginApp app)
        {
            CurrentApp = app ?? throw new ArgumentNullException(nameof(app));
        }

        /// <summary>
        /// 当前运行的插件程序实例
        /// </summary>
        public PluginApp CurrentApp { get; internal set; }

        /// <summary>
        /// 已加载的插件列表
        /// </summary>
        public List<PluginBase> LoadedPlugins { get; } = [];

        /// <summary>
        /// 主窗口，必须实现IPluginWindow接口
        /// </summary>
        public IPluginWindow Window { get; private set; }

        /// <summary>
        /// 检查插件内容类型是否合法
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool CheckPluginContentType(Type type)
        {
            if (type == null)
            {
                return false;
            }
            if (!typeof(FrameworkElement).IsAssignableFrom(type))
            {
                return false;
            }
            if (type.GetConstructor(Type.EmptyTypes) == null)
            {
                return false;
            }
            return true;
        }

        internal void LoadPreDefinedPluginsFromConfig(PluginAppConfig globalAppConfig)
        {
            foreach(var plugin in globalAppConfig.Plugins)
            {
                if(!CheckPluginContentType(plugin.ContentType))
                {
                    CurrentApp.PushException(new ArgumentException("ContentType must be a FrameworkElement type with a parameterless constructor."));
                    continue;
                }
                LoadedPlugins.Add(plugin);
            }
        }

        internal async void Start()
        {
            // 监视主窗口是否加载
            while(CurrentApp.MainWindow == null || !CurrentApp.MainWindow.IsLoaded)
            {
                await Task.Delay(100);
            }
            if(CurrentApp.MainWindow is not IPluginWindow pWnd)
            {
                return;
            }
            Window = pWnd;
            var sideMenuPlugins = new List<IPlugin>();
            var bannerPlugins = new List<IPlugin>();
            foreach(var plugin in LoadedPlugins)
            {
                plugin.OnLoading();
                if (plugin.Type == PluginTypes.SideMenu)
                {
                    sideMenuPlugins.Add(plugin);
                }
                else if (plugin.Type == PluginTypes.Banner)
                {
                    bannerPlugins.Add(plugin);
                }
            }
            // 初始化所有插件
            if (Window.Sidebar is not null)
            {
                var collection = new ObservableCollection<SideMenuItemViewModel>();
                Window.Sidebar.Style = Application.Current.FindResource("PluginWindowSideMenuStyle") as Style;
                Window.Sidebar.ItemsSource = collection;
                Window.Sidebar.SelectionChanged += Sidebar_SelectionChanged;
                foreach (var plugin in sideMenuPlugins)
                {
                    if(plugin.ContentType is null)
                    {
                        continue;
                    }
                    var item = new SideMenuItemViewModel
                    {
                        Header = plugin.Header,
                        Icon = plugin.Icon
                    };
                    _loadedPluginContentTypes.Add(item.Uid, plugin.ContentType);
                    collection.Add(item);
                }
            }

            if(Window.Banner is not null)
            {
                // var collection = new ObservableCollection<BannerItemViewModel>();
                Window.Banner.Style = Application.Current.FindResource("PluginWindowBannerStyle") as Style;
                //Window.Banner.ItemsSource = collection;
                //foreach (var plugin in bannerPlugins)
                //{
                //    if (plugin.ContentType is null)
                //    {
                //        continue;
                //    }
                //    var item = new BannerItemViewModel
                //    {
                //        Header = plugin.Header,
                //        Icon = plugin.Icon
                //    };
                //    collection.Add(item);
                //}
            }

            await CurrentApp.Dispatcher.BeginInvoke(Window.OnPluginWindowInitialize);
        }

        private readonly Dictionary<string, Type> _loadedPluginContentTypes = new();
        private readonly Dictionary<string, FrameworkElement> _loadedPluginContents = new();

        private void Sidebar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sideBar = sender as ListBox;
            if (sideBar.SelectedItem is not SideMenuItemViewModel selectedItem)
            {
                return;
            }
            if(!_loadedPluginContentTypes.TryGetValue(selectedItem.Uid, out var contentType))
            {
                return;
            }
            if (!_loadedPluginContents.TryGetValue(selectedItem.Uid, out var content))
            {
                content = Activator.CreateInstance(contentType) as FrameworkElement;
                if (content is null)
                {
                    return;
                }
                _loadedPluginContents.Add(selectedItem.Uid, content);
            }
            Window.ContentArea.Children.Clear();
            Window.ContentArea.Children.Add(content);
        }
    }
}
