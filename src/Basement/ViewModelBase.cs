using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Mph.WPFAppPlugin.Basement
{
    /// <summary>
    /// MVVM 基础 ViewModel，提供属性变更通知支持。
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 唯一标识符。
        /// </summary>
        public string Uid { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 用户自定义数据。
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// 通知属性变更。
        /// </summary>
        /// <param name="propertyName">属性名</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var args = new PropertyChangedEventArgs(propertyName);
            OnPropertyChanged(args);
            PropertyChanged?.Invoke(this, args);
        }

        /// <summary>
        /// 通知属性变更事件，可被重写以添加自定义逻辑。
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {

        }

        /// <summary>
        /// 设置属性值并自动通知变更。
        /// </summary>
        /// <typeparam name="T">属性类型</typeparam>
        /// <param name="field">字段引用</param>
        /// <param name="value">新值</param>
        /// <param name="propertyName">属性名</param>
        /// <returns>值是否发生变化</returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
