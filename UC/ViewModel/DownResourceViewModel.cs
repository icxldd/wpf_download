﻿using GalaSoft.MvvmLight;
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
                MaterialCategoryList = new ObservableCollection<CategoryTextImage>(GlobalData.api.GetMaterialCategory());
            }

        }

        private void selectWin(string pageStr)
        {
            IsShowLoadUI = true;

            UserControl showPage = null;
            if (pageStr.Equals(UCConst.PageConstName.ClickProduct))
            {
                if (GlobalData.ProductPage == null)
                {
                    initdata(pageStr);
                    showPage = new Product();
                    GlobalData.ProductPage = showPage;
                }
                else
                {

                    showPage = GlobalData.ProductPage;
                    showPage.DataContext = GlobalData.ProductPage.DataContext;
                }
            }
            else if (pageStr.Equals(UCConst.PageConstName.ClickTexture))
            {
                if (GlobalData.TexturePage == null)
                {
                    initdata(pageStr);
                    showPage = new Texture();
                    GlobalData.TexturePage = showPage;
                }
                else
                {
                    showPage = GlobalData.TexturePage;
                    showPage.DataContext = GlobalData.TexturePage.DataContext;
                }
            }
            IsShowLoadUI = false;
            win.content.Content = showPage;
            win.coltop.Height = new System.Windows.GridLength(0);
            win.top.Visibility = System.Windows.Visibility.Hidden;
            win.colbottom.Height = System.Windows.GridLength.Auto;

        }
        public RelayCommand<string> SelectProductCommand { get; set; }

        private bool _IsShowLoadUI;
        public bool IsShowLoadUI { get { return _IsShowLoadUI; } set { _IsShowLoadUI = value; RaisePropertyChanged(); } }

        public RelayCommand<CategoryTextImage> SelectedCategoryCommand { get; set; }

        public RelayCommand<CategoryTextImage> SelectedMaterialCategoryCommand { get; set; }




        private ObservableCollection<CategoryTextImage> _CategoryList = new ObservableCollection<CategoryTextImage>();

        public ObservableCollection<CategoryTextImage> CategoryList
        {
            get { return _CategoryList; }
            set { _CategoryList = value; RaisePropertyChanged(); }
        }




        private ObservableCollection<CategoryTextImage> _MaterialCategoryList = new ObservableCollection<CategoryTextImage>();

        public ObservableCollection<CategoryTextImage> MaterialCategoryList
        {
            get { return _MaterialCategoryList; }
            set { _MaterialCategoryList = value; RaisePropertyChanged(); }
        }
    }
}
