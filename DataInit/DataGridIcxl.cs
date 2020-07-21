using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFDemo.DataInit
{
    public class StudentInfo
    {
        public string Name { get; set; }
        public string Class { get; set; }
        public string Sex { get; set; }
        public int ClassRank { get; set; }
        public int SchoolRank { get; set; }
    }
    public class DataGridIcxl
    {
        private static DataGridIcxl dataInit;

        public static DataGridIcxl Instance
        {
            get
            {
                if (dataInit == null)
                    dataInit = new DataGridIcxl();
                return dataInit;
            }
        }

        private DataGridIcxl()
        {
            StudentList = new List<StudentInfo>()
            {
                new StudentInfo()
                {
                    Name="张三",
                    Class="三班",
                    Sex="男",
                    ClassRank=10,
                    SchoolRank=103
                },
                new StudentInfo()
                {
                    Name="李四",
                    Class="三班",
                    Sex="男",
                    ClassRank=12,
                    SchoolRank=110
                },
                new StudentInfo()
                {
                    Name="李梅",
                    Class="三班",
                    Sex="女",
                    ClassRank=3,
                    SchoolRank=70
                },
            };
        }
        public List<StudentInfo> StudentList { get; set; }
    }
}
