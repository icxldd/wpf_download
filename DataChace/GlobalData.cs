using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPFDemo.API;
using WPFDemo.UC.DownPage;

namespace WPFDemo.DataChace
{
    public class GlobalData
    {
        public static MainWindow MainWindow { get; set; }
        public static API.API api = new API.API();



        public static UserControl ProductPage = null;

        public static UserControl TexturePage = null;

    }
}
