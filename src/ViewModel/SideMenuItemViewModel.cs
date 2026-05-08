using Mph.WPFAppPlugin.Basement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Mph.WPFAppPlugin.ViewModel
{
    /// <summary>
    /// 侧边栏菜单项的 ViewModel 基类。
    /// </summary>
    public class SideMenuItemViewModel : ViewModelBase
    {
        public string Header
        {
            get => m_Header;
            set => SetProperty(ref m_Header, value);
        }

        /// <summary>
        /// 菜单项图标
        /// </summary>
        public ImageSource Icon
        {
            get => m_Icon;
            set => SetProperty(ref m_Icon, value);
        }

        private ImageSource m_Icon;
        private string m_Header = "My Sidemenu Item";
    }
}
