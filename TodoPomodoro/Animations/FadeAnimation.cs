using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace TodoPomodoro.Animations
{
    /// <summary>
    /// 提供淡入淡出动画效果
    /// </summary>
    public static class FadeAnimation
    {
        /// <summary>
        /// 应用淡入动画到指定元素
        /// </summary>
        /// <param name="element">要应用动画的元素</param>
        /// <param name="duration">动画持续时间（毫秒）</param>
        /// <param name="completed">动画完成后的回调</param>
        public static void FadeIn(UIElement element, double duration = 300, Action completed = null)
        {
            // 设置初始透明度
            element.Opacity = 0;
            element.Visibility = Visibility.Visible;

            // 创建动画
            DoubleAnimation animation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(duration)
            };

            // 添加完成事件
            if (completed != null)
            {
                animation.Completed += (s, e) => completed();
            }

            // 开始动画
            element.BeginAnimation(UIElement.OpacityProperty, animation);
        }

        /// <summary>
        /// 应用淡出动画到指定元素
        /// </summary>
        /// <param name="element">要应用动画的元素</param>
        /// <param name="duration">动画持续时间（毫秒）</param>
        /// <param name="completed">动画完成后的回调</param>
        public static void FadeOut(UIElement element, double duration = 300, Action completed = null)
        {
            // 创建动画
            DoubleAnimation animation = new DoubleAnimation
            {
                From = element.Opacity,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(duration)
            };

            // 添加完成事件
            if (completed != null)
            {
                animation.Completed += (s, e) => completed();
            }
            else
            {
                animation.Completed += (s, e) => element.Visibility = Visibility.Collapsed;
            }

            // 开始动画
            element.BeginAnimation(UIElement.OpacityProperty, animation);
        }
    }
}