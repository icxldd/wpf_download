using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPFDemo.DataChace;

namespace WPFDemo.UC.ViewModel
{
    public class LeftMenuViewModel : ViewModelBase
    {
        private readonly leftMenu win;
        public LeftMenuViewModel(leftMenu win)
        {

            this.win = win;
            InitEvent();
            InitData();
        }
        private void InitData()
        {
            selectWin(UCConst.PageConstName.DownResource);

        }
        private void InitEvent()
        {
            CloseWindowCommand = new RelayCommand(() =>
            {
                System.Environment.Exit(0);
            });

            SelectWindowCommand = new RelayCommand<string>(x =>
            {

                if (x.Equals(UCConst.PageConstName.DownRecord))
                {
                    selectWin(UCConst.PageConstName.DownRecord);

                }
                else if (x.Equals(UCConst.PageConstName.DownResource))
                {
                    selectWin(UCConst.PageConstName.DownResource);
                }
            });
        }

        private void selectWin(string pageStr)
        {
            UserControl showPage = null;
            if (pageStr.Equals(UCConst.PageConstName.DownRecord))
            {
                showPage = new DownRecord();

            }
            else if (pageStr.Equals(UCConst.PageConstName.DownResource))
            {
                showPage = new DownResource();
            }
            win.ShowPage.Content = showPage;
        }


        public RelayCommand CloseWindowCommand { get; set; }



        public RelayCommand<string> SelectWindowCommand { get; set; }
    }
}
