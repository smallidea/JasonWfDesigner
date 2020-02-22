// ******************************************************************
// ** Copyright：Copyright (c) 2020
// ** Project：JasonWfDesigner.Core
// ** Create Date：2020-02-21 14:31
// ** Created by：陈晓平
// ** Blog：http://smallidea.cnblogs.com
// ** Git：http://smallidea.github.com
// ** Email: smallidea@126.com
// ** Version：v 1.0
// ** Last Modified: 2020-02-21 15:54
// ** Desc：ImageUrlConverter.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace JasonWfDesigner.Core.Converters
{
    [ValueConversion(typeof(string), typeof(ImageSource))]
    public class ImageUrlConverter : IValueConverter
    {
        static ImageUrlConverter()
        {
            Instance = new ImageUrlConverter();
        }

        public static ImageUrlConverter Instance { get; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var imagePath = new Uri(value.ToString(), UriKind.RelativeOrAbsolute);
            ImageSource source = new BitmapImage(imagePath);
            return source;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}