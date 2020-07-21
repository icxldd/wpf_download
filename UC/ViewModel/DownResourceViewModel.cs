using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPFDemo.API;
using WPFDemo.DataChace;
using WPFDemo.UC.DownPage;

namespace WPFDemo.UC.ViewModel
{
    public class DownResourceViewModel : ViewModelBase
    {

        private readonly DownResource win;
        public DownResourceViewModel(DownResource win)
        {
            SelectProductCommand = new RelayCommand<string>((x) =>
            {
                selectWin(x);
            });

            SelectedCategoryCommand = new RelayCommand<CategoryTextImage>((x) =>
            {
                API.API selfApi = new API.API();
                _ = selfApi.UpdateLocalFiles(x);

            });


            SelectedMaterialCategoryCommand = new RelayCommand<CategoryTextImage>(x =>
            {
                API.API selfApi = new API.API();


                _ = selfApi.UpdateLocalFilesByMaterial(x);
            });

            this.win = win;


        }

        private void initdata(string pageStr)
        {


            if (pageStr.Equals(UCConst.PageConstName.ClickProduct))
            {
                CategoryList = new ObservableCollection<CategoryTextImage>(GlobalData.api.GetProductCategory());
            }
            else if (pageStr.Equals(UCConst.PageConstName.ClickTexture))
            {

                CategoryList = new ObservableCollection<CategoryTextImage>(GlobalData.api.GetMaterialCategory());

            }

        }

        private void selectWin(string pageStr)
        {
            initdata(pageStr);
            UserControl showPage = null;
            if (pageStr.Equals(UCConst.PageConstName.ClickProduct))
            {
                showPage = new Product();
            }
            else if (pageStr.Equals(UCConst.PageConstName.ClickTexture))
            {
                showPage = new Texture();
            }
            win.content.Content = showPage;
            win.coltop.Height = new System.Windows.GridLength(0);
            win.top.Visibility = System.Windows.Visibility.Hidden;
            win.colbottom.Height = System.Windows.GridLength.Auto;

        }
        public RelayCommand<string> SelectProductCommand { get; set; }



        public RelayCommand<CategoryTextImage> SelectedCategoryCommand { get; set; }


        public RelayCommand<CategoryTextImage> SelectedMaterialCategoryCommand { get; set; }




        private ObservableCollection<CategoryTextImage> _CategoryList = new ObservableCollection<CategoryTextImage>();

        public ObservableCollection<CategoryTextImage> CategoryList
        {
            get { return _CategoryList; }
            set { _CategoryList = value; RaisePropertyChanged(); }
        }


    }
}
