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
    public class PluginAppStartingConfig
    {
        internal PluginAppStartingConfig()
        {
        }

        /// <summary>
        /// 是否需要显示默认的异常捕获对话框
        /// </summary>
        public bool NeedShowDefaultDialog { get; set; } = true;
    }
}
