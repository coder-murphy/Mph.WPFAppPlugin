using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mph.WPFAppPlugin.Basement
{
    public class HeaderViewModel : ViewModelBase
    {
        public HeaderViewModel() { }

        /// <summary>
        /// 标题
        /// </summary>
        public string Header
        {
            get => m_Header;
            set => SetProperty(ref m_Header, value);
        }

        private string m_Header;
    }
}
