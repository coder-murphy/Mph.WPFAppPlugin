using System;
using System.Windows.Media;

namespace Mph.WPFAppPlugin.Interfaces
{
    /// <summary>
    /// 插件接口
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// 插件唯一标识
        /// </summary>
        string Guid { get; }

        /// <summary>
        /// 插件名称
        /// </summary>
        string Header { get; set; }

        /// <summary>
        /// 插件内容类型,必须是一个派生自FrameworkElement的类型，并且有一个无参构造函数
        /// </summary>
        Type ContentType { get; }

        /// <summary>
        /// 插件显示的图标
        /// </summary>
        ImageSource Icon { get; }

        /// <summary>
        /// 插件类型
        /// </summary>
        PluginTypes Type { get; }

        /// <summary>
        /// 插件加载时调用
        /// </summary>
        void OnLoading();
    }
}
