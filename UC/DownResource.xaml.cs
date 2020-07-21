using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFDemo.UC
{
    /// <summary>
    /// DownResource.xaml 的交互逻辑
    /// </summary>
    public partial class DownResource : UserControl
    {
        public DownResource()
        {
            InitializeComponent();
            this.DataContext = new WPFDemo.UC.ViewModel.DownResourceViewModel(this);
        }
    }
}
