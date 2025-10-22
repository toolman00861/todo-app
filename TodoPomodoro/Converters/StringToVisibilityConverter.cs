using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TodoPomodoro.Converters
{
    /// <summary>
    /// 将字符串转换为可见性的转换器
    /// </summary>
    public class StringToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// 将字符串转换为可见性
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                return string.IsNullOrWhiteSpace(stringValue) ? Visibility.Collapsed : Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        /// <summary>
        /// 将可见性转换为字符串（不实现）
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}