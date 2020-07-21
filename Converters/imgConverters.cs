using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFDemo.API;
using static System.Net.Mime.MediaTypeNames;

namespace WPFDemo.Converters
{
    public class imgConverters : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var obj = (value as CategoryTextImage);
            if (obj.IsCompleteDown)
            {
                return obj.ImageUrl;
            }
            else
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(obj.ImageUrl));
                FormatConvertedBitmap newFormatedBitmapSource = new FormatConvertedBitmap();
                newFormatedBitmapSource.BeginInit();
                newFormatedBitmapSource.Source = bitmapImage;
                newFormatedBitmapSource.DestinationFormat = PixelFormats.Gray8;
                newFormatedBitmapSource.EndInit();
                return newFormatedBitmapSource;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
