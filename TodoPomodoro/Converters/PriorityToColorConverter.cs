using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using TodoPomodoro.Models;

namespace TodoPomodoro.Converters
{
    /// <summary>
    /// 将优先级转换为颜色的转换器
    /// </summary>
    public class PriorityToColorConverter : IValueConverter
    {
        /// <summary>
        /// 将优先级转换为颜色
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Priority priority)
            {
                return priority switch
                {
                    Priority.High => new SolidColorBrush(Color.FromRgb(244, 67, 54)),   // #F44336 红色
                    Priority.Medium => new SolidColorBrush(Color.FromRgb(255, 152, 0)), // #FF9800 橙色
                    Priority.Low => new SolidColorBrush(Color.FromRgb(76, 175, 80)),    // #4CAF50 绿色
                    _ => new SolidColorBrush(Colors.Gray)
                };
            }
            return new SolidColorBrush(Colors.Gray);
        }

        /// <summary>
        /// 将颜色转换为优先级（不实现）
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}