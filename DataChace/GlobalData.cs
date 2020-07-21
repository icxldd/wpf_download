using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFDemo.API;

namespace WPFDemo.DataChace
{
    public class GlobalData
    {
        public static MainWindow MainWindow { get; set; }
        public static API.API api = new API.API();

    }
}
