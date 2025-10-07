using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mph.WPFAppPlugin
{
    /// <summary>
    /// 插件程序启动配置
    /// </summary>
    public class PluginAppConfig
    {
        internal PluginAppConfig()
        {
        }

        /// <summary>
        /// 是否需要显示默认的异常捕获对话框
        /// </summary>
        public bool NeedShowDefaultDialog { get; set; } = true;

        /// <summary>
        /// 已加载的插件列表
        /// </summary>
        public IList<PluginBase> Plugins { get; } = [];

        /// <summary>
        /// 程序启动参数
        /// </summary>
        public string[] StartArgs { get; internal set; }

        public bool AddPlugin<T>() where T : PluginBase, new()
        {
            Plugins.Add(new T());
            return true;
        }
    }
}
