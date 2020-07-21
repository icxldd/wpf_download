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
using WPFDemo.DataInit;

namespace WPFDemo.UC
{
    /// <summary>
    /// DataGridIcxl.xaml 的交互逻辑
    /// </summary>
    public partial class DataGridIcxl : UserControl
    {
        public DataGridIcxl()
        {
            InitializeComponent();
            this.DataGrid.ItemsSource = DataInit.DataGridIcxl.Instance.StudentList;
        }
    }
}
