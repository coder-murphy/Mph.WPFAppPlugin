using System;

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
        string Name { get; set; }

        /// <summary>
        /// 插件内容类型,必须是一个派生自FrameworkElement的类型，并且有一个无参构造函数
        /// </summary>
        Type ContentType { get; set; }
    }
}
