using System.ComponentModel;
using System.Runtime.CompilerServices;
using TodoPomodoro.Models;

namespace TodoPomodoro.ViewModels
{
    /// <summary>
    /// 主视图模型，整合待办事项和番茄钟功能
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private TodoViewModel _todoViewModel;
        private PomodoroViewModel _pomodoroViewModel;
        private SettingsViewModel _settingsViewModel;

        /// <summary>
        /// 待办事项视图模型
        /// </summary>
        public TodoViewModel TodoViewModel
        {
            get => _todoViewModel;
            set
            {
                _todoViewModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 番茄钟视图模型
        /// </summary>
        public PomodoroViewModel PomodoroViewModel
        {
            get => _pomodoroViewModel;
            set
            {
                _pomodoroViewModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 设置视图模型
        /// </summary>
        public SettingsViewModel SettingsViewModel
        {
            get => _settingsViewModel;
            set
            {
                _settingsViewModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainViewModel()
        {
            _todoViewModel = new TodoViewModel();
            _pomodoroViewModel = new PomodoroViewModel();
            _settingsViewModel = new SettingsViewModel();

            // 关联待办事项和番茄钟
            _todoViewModel.PropertyChanged += TodoViewModel_PropertyChanged;

            // 订阅设置变更事件
            _settingsViewModel.SettingsChanged += OnSettingsChanged;
        }

        private void TodoViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TodoViewModel.SelectedTodoItem))
            {
                // 当选择待办事项时，将其关联到番茄钟
                _pomodoroViewModel.Timer.CurrentTodoItem = _todoViewModel.SelectedTodoItem;
            }
        }

        /// <summary>
        /// 处理设置变更
        /// </summary>
        private void OnSettingsChanged(object? sender, SettingsChangedEventArgs e)
        {
            _pomodoroViewModel.Timer.UpdateSettings(
                e.WorkDuration,
                e.ShortBreakDuration,
                e.LongBreakDuration,
                e.PomodorosUntilLongBreak
            );
        }

        /// <summary>
        /// 属性变更事件
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// 触发属性变更事件
        /// </summary>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}