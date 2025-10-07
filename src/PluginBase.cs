using Mph.WPFAppPlugin.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Mph.WPFAppPlugin
{
    public enum PluginTypes
    {
        SideMenu,

        Banner,
    }

    /// <summary>
    /// 插件基类
    /// </summary>
    public abstract class PluginBase : IPlugin
    {
        public string Guid { get; } = System.Guid.NewGuid().ToString("N");

        public virtual string Header { get; set; } = "My Plugin";

        public abstract Type ContentType { get; }

        public virtual ImageSource Icon { get;}

        public virtual PluginTypes Type { get;} = PluginTypes.SideMenu;

        public abstract void OnLoading();
    }
}
