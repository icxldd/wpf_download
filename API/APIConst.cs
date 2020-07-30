using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFDemo.API
{
    public class APIConst
    {

        public const string GetToken = "/token";//获取TOKEN


        //产品
        public const string GetProductCategory = "/Category?type=product&organId=";//获取分类
        public const string GetProductObj = "/Products/{0}";//获取产品 {0}=ID
        public const string GetProductByCategoryID = "/products?page=1&pageSize=10&orderBy=Name&desc=false&search=&categoryId={0}&classify=true";//获取产品根据分类ID
        public const string GetProductByCategoryID_2 = "/products?page=1&pageSize={1}&orderBy=Name&desc=false&search=&categoryId={0}&classify=true";//获取产品根据分类ID

        //材质

        public const string GetMd5IsExist = "/files/MD5/{0}";//





        //产品
        public const string GetMaterialCategory = "/Category?type=material&organId=";//获取分类
        public const string GetMaterialObj = "/material/{0}";//获取产品 {0}=ID
        public const string GetMaterialByCategoryID = "/material?page=1&pageSize=10&orderBy=Name&desc=false&search=&categoryId={0}&classify=true";//获取产品根据分类ID
        public const string GetMaterialByCategoryID_2 = "/material?page=1&pageSize={1}&orderBy=Name&desc=false&search=&categoryId={0}&classify=true";//获取产品根据分类ID





        public static string defulatMaterialImagePath = Environment.CurrentDirectory + "/Images/MaterialDefaultImg.jpg";
        public static string defulatMaterialImagePath_2 = Environment.CurrentDirectory + "/Images/MaterialDefaultImg_2.jpg";



        public static double defalutOpacity = 0.3;
    }
}
