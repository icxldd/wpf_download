using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPFDemo.Converters
{
    public class ImgMultiConverters : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null)
            {
                if (values.Length == 2)
                {
                    string imgurl = values[0].ToString();
                    bool isCompse = System.Convert.ToBoolean(values[1]);


                    if (isCompse)
                    {
                        BitmapImage bitmapImage = new BitmapImage(new Uri(imgurl));
                        FormatConvertedBitmap newFormatedBitmapSource = new FormatConvertedBitmap();
                        newFormatedBitmapSource.BeginInit();
                        newFormatedBitmapSource.Source = bitmapImage;
                        newFormatedBitmapSource.EndInit();
                        return newFormatedBitmapSource;
                    }
                    else
                    {
                        if (imgurl.Contains("http"))
                        {


                            BitmapImage bitmapImage = new BitmapImage(new Uri(imgurl));
                            FormatConvertedBitmap newFormatedBitmapSource = new FormatConvertedBitmap();
                            newFormatedBitmapSource.BeginInit();
                            newFormatedBitmapSource.Source = bitmapImage;
                            newFormatedBitmapSource.DestinationFormat = PixelFormats.Gray8;
                            newFormatedBitmapSource.EndInit();
                            return newFormatedBitmapSource;
                        }
                        else
                        {


                            BitmapImage bitmapImage = new BitmapImage(new Uri(WPFDemo.API.APIConst.defulatMaterialImagePath_2));
                            FormatConvertedBitmap newFormatedBitmapSource = new FormatConvertedBitmap();
                            newFormatedBitmapSource.BeginInit();
                            newFormatedBitmapSource.Source = bitmapImage;
                            newFormatedBitmapSource.EndInit();
                            return newFormatedBitmapSource;
                        }
                    }
                }
            }
            return "";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
