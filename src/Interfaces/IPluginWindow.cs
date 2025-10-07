using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Mph.WPFAppPlugin.Interfaces
{
    public interface IPluginWindow
    {
        /// <summary>
        /// 横幅控件容器
        /// </summary>
        ListView Banner { get; }

        /// <summary>
        /// 侧边栏控件容器
        /// </summary>
        ListBox Sidebar { get; }

        /// <summary>
        /// 内容显示区域
        /// </summary>
        Panel ContentArea { get; }

        /// <summary>
        /// 窗口初始化时调用
        /// </summary>
        void OnPluginWindowInitialize();
    }
}
