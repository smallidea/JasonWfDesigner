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
// ** Desc：CanNullConverter.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace JasonWfDesigner.Core.Converters
{
    public class CanNullConverter : IValueConverter
    {
        static CanNullConverter()
        {
            Instance = new CanNullConverter();
        }

        public static CanNullConverter Instance { get; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var toType = targetType;
            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var nullableConvert = new NullableConverter(targetType);
                toType = nullableConvert.UnderlyingType;
            }

            return value != null && value.GetType() == toType ? value : null;
        }
    }
}