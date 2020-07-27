using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EasyHttp.Http;
using GalaSoft.MvvmLight;
using WPFDemo.Common.Encryption;
using Newtonsoft.Json;
using WPFDemo.LocalDB;

namespace WPFDemo.API
{


    public class CategoryTextImage : ViewModelBase
    {

        public string _CategoryID;
        public string _ImageUrl;
        public string _CategoryText;
        public bool _ProgressBarIsShow = false;
        public double _ProgressBarVal = 0;
        public bool _IsCompleteDown = false;

        public string CategoryID { get { return _CategoryID; } set { _CategoryID = value; RaisePropertyChanged(); } }
        public string ImageUrl { get { return _ImageUrl; } set { _ImageUrl = value; RaisePropertyChanged(); } }
        public string CategoryText { get { return _CategoryText; } set { _CategoryText = value; RaisePropertyChanged(); } }
        public bool ProgressBarIsShow { get { return _ProgressBarIsShow; } set { _ProgressBarIsShow = value; RaisePropertyChanged(); } }
        public double ProgressBarVal { get { return _ProgressBarVal; } set { _ProgressBarVal = value; RaisePropertyChanged(); } }
        public bool IsCompleteDown { get { return _IsCompleteDown; } set { _IsCompleteDown = value; RaisePropertyChanged(); } }
    }
    public class API
    {
        private string accout { get; set; }
        private string passwd { get; set; }
        private static string token = null;
        private string url { get; set; }
        private EasyHttp.Http.HttpClient http = new EasyHttp.Http.HttpClient();
        public API()
        {
            accout = ConfigurationManager.ConnectionStrings["accout"].ToString();
            passwd = ConfigurationManager.ConnectionStrings["passwd"].ToString();
            url = ConfigurationManager.ConnectionStrings["serverUrl"].ToString();
            login(accout, passwd);
        }

        public bool login(string accout, string passwd)
        {
            try
            {
                if (token != null)
                {
                    http.Request.AddExtraHeader("Authorization", token);
                    return true;
                }
                else
                {
                    dynamic customer = new { account = accout, password = MD5.CalcString(passwd) }; // Or any dynamic type
                    HttpResponse a = http.Post(url + APIConst.GetToken, customer, HttpContentTypes.ApplicationJson);
                    var dd = a.DynamicBody;
                    if (!string.IsNullOrEmpty(dd.token))
                    {
                        token = "bearer " + dd.token;
                        http.Request.AddExtraHeader("Authorization", token);
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
        private class fileItemObj
        {

            public string serverUrl { get; set; }


            public string localUrl { get; set; }

            public string houzui { get; set; }

            public string name { get; set; }

        }
        public async Task<bool> HttpDownload(string url, string path)
        {
            string tempPath = System.IO.Path.GetDirectoryName(path) + @"\temp";
            System.IO.Directory.CreateDirectory(tempPath);  //创建临时文件目录
            string tempFile = tempPath + @"\" + System.IO.Path.GetFileName(path) + ".temp"; //临时文件
            if (System.IO.File.Exists(tempFile))
            {
                System.IO.File.Delete(tempFile);    //存在则删除
            }
            try
            {
                FileStream fs = new FileStream(tempFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                // 设置参数
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //发送请求并获取相应回应数据
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                Stream responseStream = response.GetResponseStream();
                //创建本地文件写入流
                //Stream stream = new FileStream(tempFile, FileMode.Create);
                byte[] bArr = new byte[1024];
                int size = await responseStream.ReadAsync(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    //stream.Write(bArr, 0, size);
                    await fs.WriteAsync(bArr, 0, size);
                    size = await responseStream.ReadAsync(bArr, 0, (int)bArr.Length);
                }
                //stream.Close();
                fs.Close();
                responseStream.Close();
                System.IO.File.Move(tempFile, path);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private async Task downFile(string serverUrl, string localFileUrl)
        {
            await HttpDownload(serverUrl, localFileUrl);
        }
        public async Task UpdateLocalFiles(CategoryTextImage Category)
        {

            await Task.Run(async () =>
           {
               UpdateRecord uDao = new UpdateRecord();
               Category.ProgressBarIsShow = true;
               Category.ProgressBarVal = 0;
               //1.GET /products?categoryId=00UGEYYE86PP30V&pageSize=33&page=1&search= HTTP/1.1   根据分页加载所有产品
               var RequestResult = http.Get(url + string.Format(APIConst.GetProductByCategoryID, Category.CategoryID)).DynamicBody;
               int MaxCount = RequestResult.total;
               var RequestResult_2 = http.Get(url + string.Format(APIConst.GetProductByCategoryID_2, Category.CategoryID, MaxCount)).DynamicBody;

               //1.2 收集产品ID
               List<string> productIdS = new List<string>();
               foreach (var item in RequestResult_2.data)
               {
                   productIdS.Add(item.id);
               }
               Category.ProgressBarVal = 5;
               //2.GET /products/Y6UN35PAVZMYPP6 HTTP/1.1
               foreach (var item in productIdS)
               {

                   List<fileItemObj> fileDics = new List<fileItemObj>();
                   var v1 = http.Get(url + string.Format(APIConst.GetProductObj, item)).DynamicBody;
                   string url_ = v1.specifications[0].staticMeshes[0].url + "," + v1.specifications[0].staticMeshes[0].packageName;
                   string dependencies = v1.specifications[0].staticMeshes[0].dependencies;
                   List<string> arr = dependencies.Split(';').ToList();
                   arr.Add(url_);
                   foreach (string item_2 in arr)
                   {

                       fileItemObj f = new fileItemObj();
                       if (string.IsNullOrEmpty(item_2))
                       {
                           continue;
                       }
                       List<string> a = item_2.Split(',').ToList();
                       f.localUrl = a[1];
                       f.serverUrl = a[0];
                       f.houzui = "." + a[0].Split('.')[1];
                       f.name = v1.specifications[0].staticMeshes[0].packageName;

                       fileDics.Add(f);
                   }

                   double i = 100 / productIdS.Count;
                   string baseUrl = Environment.CurrentDirectory;
                   //比较是否需要下载（更新）
                   foreach (fileItemObj obj in fileDics)
                   {
                       string serverUrl, localUrl;
                       string newLocal = obj.localUrl.Replace("Game", "Content");
                       string filePath = (baseUrl + "/AZMJ" + newLocal).Replace("/", "\\");
                       double oneI = i / fileDics.Count;
                       serverUrl = url + obj.serverUrl;
                       localUrl = filePath;
                       string FileFullName = filePath + obj.houzui;
                       if (File.Exists(FileFullName))
                       {
                           string md5 = MD5.CalcFile(FileFullName);

                           try
                           {
                               var vvv1 = http.Get(url + string.Format(APIConst.GetMd5IsExist, md5)).DynamicBody;
                               string id = vvv1.id;
                               //跳过不需要更新
                               continue;
                           }
                           catch (Exception)
                           {
                               //执行下载
                               await downFile(serverUrl, FileFullName);

                           }
                       }
                       else
                       {
                           await downFile(serverUrl, FileFullName);

                       }

                       Category.ProgressBarVal += Convert.ToDouble(decimal.Round(decimal.Parse(oneI.ToString()), 2));
                   }
               }
               Category.ProgressBarVal = 100;
               Category.ProgressBarIsShow = false;
               Category.IsCompleteDown = true;
               uDao.insert(new UpdateRecoreModel() { CategoryId = Category.CategoryID, Type = eCategoryType.产品, Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
           });
        }

        public async Task UpdateLocalFilesByMaterial(CategoryTextImage Category)
        {
            await Task.Run(async () =>
            {
                UpdateRecord uDao = new UpdateRecord();
                Category.ProgressBarIsShow = true;
                Category.ProgressBarVal = 0;
                var RequestResult = http.Get(url + string.Format(APIConst.GetMaterialByCategoryID, Category.CategoryID)).DynamicBody;
                int MaxCount = RequestResult.total;
                var RequestResult_2 = http.Get(url + string.Format(APIConst.GetMaterialByCategoryID_2, Category.CategoryID, MaxCount)).DynamicBody;

                List<string> MaterialIdS = new List<string>();
                foreach (var item in RequestResult_2.data)
                {
                    MaterialIdS.Add(item.id);
                }
                Category.ProgressBarVal = 5;
                foreach (var item in MaterialIdS)
                {

                    List<fileItemObj> fileDics = new List<fileItemObj>();
                    var v1 = http.Get(url + string.Format(APIConst.GetMaterialObj, item)).DynamicBody;



                    fileItemObj f = new fileItemObj();
                    f.localUrl = v1.packageName;
                    f.serverUrl = v1.url;
                    f.houzui = "." + v1.url.Split('.')[1];
                    //f.name = v1.specifications[0].staticMeshes[0].packageName;

                    fileDics.Add(f);

                    double i = 100d / MaterialIdS.Count;
                    string baseUrl = Environment.CurrentDirectory;
                    //比较是否需要下载（更新）
                    foreach (fileItemObj obj in fileDics)
                    {
                        string serverUrl, localUrl;
                        string newLocal = obj.localUrl.Replace("Game", "Content");
                        string filePath = (baseUrl + "/AZMJ" + newLocal).Replace("/", "\\");
                        double oneI = i / fileDics.Count;
                        serverUrl = url + obj.serverUrl;
                        localUrl = filePath;
                        string FileFullName = filePath + obj.houzui;
                        if (File.Exists(FileFullName))
                        {
                            string md5 = MD5.CalcFile(FileFullName);

                            try
                            {
                                var vvv1 = http.Get(url + string.Format(APIConst.GetMd5IsExist, md5)).DynamicBody;
                                string id = vvv1.id;
                                //跳过不需要更新
                                goto end;
                            }
                            catch (Exception)
                            {
                                //执行下载
                                await downFile(serverUrl, FileFullName);

                            }
                        }
                        else
                        {
                            await downFile(serverUrl, FileFullName);

                        }
                    end:
                        Category.ProgressBarVal += oneI;
                    }
                }
                Category.ProgressBarVal = 100;
                Category.ProgressBarIsShow = false;
                Category.IsCompleteDown = true;
                uDao.insert(new UpdateRecoreModel() { CategoryId = Category.CategoryID, Type = eCategoryType.材质, Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
            });
        }
        private IList<CategoryTextImage> GetCategoryList(dynamic obj)
        {
            UpdateRecord dao = new UpdateRecord();
            IList<CategoryTextImage> rlsit = new List<CategoryTextImage>();
            foreach (var CategoryObj in obj.children)
            {
                try
                {
                    CategoryTextImage selfoBj = new CategoryTextImage();

                    var RequestResult = http.Get(url + string.Format(APIConst.GetProductByCategoryID, CategoryObj.id)).DynamicBody;
                    var data = RequestResult.data;
                    int len = BuildRanDomNumber(data.Length);
                    selfoBj.ImageUrl = url + data[len].icon;
                    selfoBj.CategoryText = CategoryObj.name;
                    selfoBj.CategoryID = CategoryObj.id;
                    selfoBj.IsCompleteDown = dao.find(CategoryObj.id);
                    rlsit.Add(selfoBj);
                }
                catch (Exception)
                {

                }
            }


            return rlsit.OrderByDescending(x => x.IsCompleteDown == true).ToList();
        }



        private IList<CategoryTextImage> GetCategoryListByMaterial(dynamic obj)
        {
            IList<CategoryTextImage> rlsit = new List<CategoryTextImage>();
            foreach (var CategoryObj in obj.children)
            {
                try
                {
                    CategoryTextImage selfoBj = new CategoryTextImage();

                    var RequestResult = http.Get(url + string.Format(APIConst.GetMaterialByCategoryID, CategoryObj.id)).DynamicBody;
                    var data = RequestResult.data;
                    if (data == null)
                    {
                        continue;
                    }
                    int len = BuildRanDomNumber(data.Length);
                    if (data[len].icon == null)
                    {
                        selfoBj.ImageUrl = APIConst.defulatMaterialImagePath;
                    }
                    else
                    {
                        selfoBj.ImageUrl = url + data[len].icon;
                    }

                    selfoBj.CategoryText = CategoryObj.name;
                    selfoBj.CategoryID = CategoryObj.id;
                    rlsit.Add(selfoBj);
                }
                catch (Exception)
                {

                }
            }
            return rlsit;
        }

        private static int BuildRanDomNumber(int chidrenLen)
        {
            Random a = new Random();
            int selfRanDomNumber = a.Next(chidrenLen);
            return selfRanDomNumber;
        }
        // 获取分类与分类图片
        public IList<CategoryTextImage> GetProductCategory()
        {
            var RequestResult = http.Get(url + APIConst.GetProductCategory).DynamicBody;
            try
            {
                var a = GetCategoryList(RequestResult);

                return a;


            }
            catch (Exception e)
            {
                return null;

            }

        }

        public List<CategoryTextImage> GetMaterialCategory()
        {
            var RequestResult = http.Get(url + APIConst.GetMaterialCategory).DynamicBody;
            try
            {

                var a = GetCategoryListByMaterial(RequestResult);

                return a;


            }
            catch (Exception e)
            {
                return null;

            }
        }


    }
}
