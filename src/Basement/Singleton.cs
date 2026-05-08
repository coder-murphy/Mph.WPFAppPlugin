using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mph.WPFAppPlugin.Basement
{
    /// <summary>
    /// 单例模式实现。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : class, new()
    {
        private static readonly Lazy<T> _instance = new(() => new T());

        /// <summary>
        /// 获取单例实例。
        /// </summary>
        public static T Instance => _instance.Value;
    }
}
