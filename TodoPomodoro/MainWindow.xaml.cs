using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TodoPomodoro.Animations;
using TodoPomodoro.Models;
using TodoPomodoro.ViewModels;

namespace TodoPomodoro;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private MainViewModel _viewModel;
    
    public MainWindow()
    {
        InitializeComponent();
        
        // 设置数据上下文
        _viewModel = new MainViewModel();
        DataContext = _viewModel;
        
        // 添加拖动窗口的事件处理
        this.MouseLeftButtonDown += (s, e) =>
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        };
        
        // 订阅待办事项事件
        _viewModel.TodoViewModel.TodoItemAdded += TodoViewModel_TodoItemAdded;
        _viewModel.TodoViewModel.TodoItemDeleted += TodoViewModel_TodoItemDeleted;
        _viewModel.TodoViewModel.TodoItemStatusChanged += TodoViewModel_TodoItemStatusChanged;
        
        // 订阅番茄钟事件
        _viewModel.PomodoroViewModel.Timer.PomodoroCompleted += Timer_PomodoroCompleted;
        _viewModel.PomodoroViewModel.Timer.BreakCompleted += Timer_BreakCompleted;
        
        // 窗口加载完成后的动画
        this.Loaded += MainWindow_Loaded;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        // 窗口加载动画
        ScaleAnimation.Scale(this, 0.95, 1.0, 300);
    }

    private void Timer_BreakCompleted(object sender, EventArgs e)
    {
        // 休息结束动画
        var pomodoroPanel = FindVisualChild<Grid>(this, "PomodoroPanel");
        if (pomodoroPanel != null)
        {
            ScaleAnimation.Bounce(pomodoroPanel, 800);
        }
    }

    private void Timer_PomodoroCompleted(object sender, EventArgs e)
    {
        // 番茄钟完成动画
        var pomodoroPanel = FindVisualChild<Grid>(this, "PomodoroPanel");
        if (pomodoroPanel != null)
        {
            ScaleAnimation.Bounce(pomodoroPanel, 800);
        }
    }

    private void TodoViewModel_TodoItemStatusChanged(object sender, TodoItem e)
    {
        // 状态变更动画
        var listBox = FindVisualChild<ListBox>(this, "TodoListBox");
        if (listBox != null)
        {
            var container = listBox.ItemContainerGenerator.ContainerFromItem(e) as ListBoxItem;
            if (container != null)
            {
                ScaleAnimation.Bounce(container, 500);
            }
        }
    }

    private void TodoViewModel_TodoItemDeleted(object sender, TodoItem e)
    {
        // 删除动画在视图中处理
    }

    private void TodoViewModel_TodoItemAdded(object sender, TodoItem e)
    {
        // 添加动画
        var listBox = FindVisualChild<ListBox>(this, "TodoListBox");
        if (listBox != null)
        {
            // 等待项目容器生成
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                var container = listBox.ItemContainerGenerator.ContainerFromItem(e) as ListBoxItem;
                if (container != null)
                {
                    ScaleAnimation.Scale(container, 0.8, 1.0, 300);
                }
            }));
        }
    }
    
    /// <summary>
    /// 查找视觉树中的子元素
    /// </summary>
    private static T FindVisualChild<T>(DependencyObject parent, string name = null) where T : DependencyObject
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            
            if (child is T typedChild && (string.IsNullOrEmpty(name) || (child is FrameworkElement fe && fe.Name == name)))
            {
                return typedChild;
            }
            
            var result = FindVisualChild<T>(child, name);
            if (result != null)
            {
                return result;
            }
        }
        
        return null;
    }
    
    /// <summary>
    /// 最小化按钮点击事件
    /// </summary>
    private void MinimizeButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Minimized;
    }
    
    /// <summary>
    /// 关闭按钮点击事件
    /// </summary>
    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        // 关闭前的动画
        ScaleAnimation.Scale(this, 1.0, 0.95, 200, () => this.Close());
    }
}