using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace TodoPomodoro.Animations
{
    /// <summary>
    /// 提供缩放动画效果
    /// </summary>
    public static class ScaleAnimation
    {
        /// <summary>
        /// 应用缩放动画到指定元素
        /// </summary>
        /// <param name="element">要应用动画的元素</param>
        /// <param name="fromScale">起始缩放比例</param>
        /// <param name="toScale">结束缩放比例</param>
        /// <param name="duration">动画持续时间（毫秒）</param>
        /// <param name="completed">动画完成后的回调</param>
        public static void Scale(UIElement element, double fromScale, double toScale, double duration = 300, Action completed = null)
        {
            // 确保元素有变换
            if (element.RenderTransform == null || !(element.RenderTransform is ScaleTransform))
            {
                element.RenderTransform = new ScaleTransform(1, 1);
                element.RenderTransformOrigin = new Point(0.5, 0.5);
            }

            // 创建X轴动画
            DoubleAnimation animationX = new DoubleAnimation
            {
                From = fromScale,
                To = toScale,
                Duration = TimeSpan.FromMilliseconds(duration),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            // 创建Y轴动画
            DoubleAnimation animationY = new DoubleAnimation
            {
                From = fromScale,
                To = toScale,
                Duration = TimeSpan.FromMilliseconds(duration),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            // 添加完成事件
            if (completed != null)
            {
                animationX.Completed += (s, e) => completed();
            }

            // 开始动画
            ScaleTransform transform = (ScaleTransform)element.RenderTransform;
            transform.BeginAnimation(ScaleTransform.ScaleXProperty, animationX);
            transform.BeginAnimation(ScaleTransform.ScaleYProperty, animationY);
        }

        /// <summary>
        /// 应用弹跳效果动画到指定元素
        /// </summary>
        /// <param name="element">要应用动画的元素</param>
        /// <param name="duration">动画持续时间（毫秒）</param>
        public static void Bounce(UIElement element, double duration = 500)
        {
            // 确保元素有变换
            if (element.RenderTransform == null || !(element.RenderTransform is ScaleTransform))
            {
                element.RenderTransform = new ScaleTransform(1, 1);
                element.RenderTransformOrigin = new Point(0.5, 0.5);
            }

            // 创建弹跳动画
            DoubleAnimationUsingKeyFrames animation = new DoubleAnimationUsingKeyFrames();
            
            // 添加关键帧
            animation.KeyFrames.Add(new EasingDoubleKeyFrame(1.0, KeyTime.FromTimeSpan(TimeSpan.Zero)));
            animation.KeyFrames.Add(new EasingDoubleKeyFrame(1.2, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(duration * 0.3)), new QuadraticEase { EasingMode = EasingMode.EaseOut }));
            animation.KeyFrames.Add(new EasingDoubleKeyFrame(0.9, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(duration * 0.6)), new QuadraticEase { EasingMode = EasingMode.EaseInOut }));
            animation.KeyFrames.Add(new EasingDoubleKeyFrame(1.0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(duration)), new QuadraticEase { EasingMode = EasingMode.EaseOut }));

            // 开始动画
            ScaleTransform transform = (ScaleTransform)element.RenderTransform;
            transform.BeginAnimation(ScaleTransform.ScaleXProperty, animation);
            transform.BeginAnimation(ScaleTransform.ScaleYProperty, animation);
        }
    }
}