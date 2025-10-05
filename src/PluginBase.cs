using Mph.WPFAppPlugin.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mph.WPFAppPlugin
{
    /// <summary>
    /// 插件基类
    /// </summary>
    public class PluginBase : IPlugin
    {
        public string Guid { get; } = System.Guid.NewGuid().ToString("N");

        public string Name { get; set; } = "My Plugin";

        public Type ContentType
        {
            get => m_ContentType;
            set
            {
                if(!CheckContentType(value))
                {
                    throw new ArgumentException("ContentType must be a FrameworkElement type with a parameterless constructor.");
                }
                m_ContentType = value;
            }
        }

        private Type m_ContentType = null;

        private static bool CheckContentType(Type type)
        {
            if (type == null)
            {
                return false;
            }
            if (!typeof(System.Windows.FrameworkElement).IsAssignableFrom(type))
            {
                return false;
            }
            if (type.GetConstructor(Type.EmptyTypes) == null)
            {
                return false;
            }
            return true;
        }
    }
}
