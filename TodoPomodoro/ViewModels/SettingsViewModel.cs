using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TodoPomodoro.Services;

namespace TodoPomodoro.ViewModels
{
    /// <summary>
    /// 设置页面视图模型
    /// </summary>
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private int _workDuration = 25;
        private int _shortBreakDuration = 5;
        private int _longBreakDuration = 15;
        private int _pomodorosUntilLongBreak = 4;
        private bool _isVisible = false;

        /// <summary>
        /// 工作时间（分钟）
        /// </summary>
        public int WorkDuration
        {
            get => _workDuration;
            set
            {
                if (value >= 1 && value <= 60)
                {
                    _workDuration = value;
                    OnPropertyChanged();
                    OnSettingsChanged();
                }
            }
        }

        /// <summary>
        /// 短休息时间（分钟）
        /// </summary>
        public int ShortBreakDuration
        {
            get => _shortBreakDuration;
            set
            {
                if (value >= 1 && value <= 30)
                {
                    _shortBreakDuration = value;
                    OnPropertyChanged();
                    OnSettingsChanged();
                }
            }
        }

        /// <summary>
        /// 长休息时间（分钟）
        /// </summary>
        public int LongBreakDuration
        {
            get => _longBreakDuration;
            set
            {
                if (value >= 1 && value <= 60)
                {
                    _longBreakDuration = value;
                    OnPropertyChanged();
                    OnSettingsChanged();
                }
            }
        }

        /// <summary>
        /// 几个番茄钟后进行长休息
        /// </summary>
        public int PomodorosUntilLongBreak
        {
            get => _pomodorosUntilLongBreak;
            set
            {
                if (value >= 2 && value <= 8)
                {
                    _pomodorosUntilLongBreak = value;
                    OnPropertyChanged();
                    OnSettingsChanged();
                }
            }
        }

        /// <summary>
        /// 设置面板是否可见
        /// </summary>
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 显示设置命令
        /// </summary>
        public ICommand ShowSettingsCommand { get; }

        /// <summary>
        /// 隐藏设置命令
        /// </summary>
        public ICommand HideSettingsCommand { get; }

        /// <summary>
        /// 重置为默认值命令
        /// </summary>
        public ICommand ResetToDefaultCommand { get; }
        
        // 增加/减少时间的命令
        public ICommand IncreaseWorkDurationCommand { get; }
        public ICommand DecreaseWorkDurationCommand { get; }
        public ICommand IncreaseShortBreakCommand { get; }
        public ICommand DecreaseShortBreakCommand { get; }
        public ICommand IncreaseLongBreakCommand { get; }
        public ICommand DecreaseLongBreakCommand { get; }
        public ICommand IncreasePomodoroCountCommand { get; }
        public ICommand DecreasePomodoroCountCommand { get; }

        /// <summary>
        /// 设置变更事件
        /// </summary>
        public event EventHandler<SettingsChangedEventArgs>? SettingsChanged;

        public SettingsViewModel()
        {
            ShowSettingsCommand = new RelayCommand(_ => IsVisible = true);
            HideSettingsCommand = new RelayCommand(_ => IsVisible = false);
            ResetToDefaultCommand = new RelayCommand(_ => ResetToDefault());
            
            // 增加/减少时间的命令
            IncreaseWorkDurationCommand = new RelayCommand(_ => WorkDuration = Math.Min(60, WorkDuration + 1));
            DecreaseWorkDurationCommand = new RelayCommand(_ => WorkDuration = Math.Max(1, WorkDuration - 1));
            IncreaseShortBreakCommand = new RelayCommand(_ => ShortBreakDuration = Math.Min(30, ShortBreakDuration + 1));
            DecreaseShortBreakCommand = new RelayCommand(_ => ShortBreakDuration = Math.Max(1, ShortBreakDuration - 1));
            IncreaseLongBreakCommand = new RelayCommand(_ => LongBreakDuration = Math.Min(60, LongBreakDuration + 1));
            DecreaseLongBreakCommand = new RelayCommand(_ => LongBreakDuration = Math.Max(1, LongBreakDuration - 1));
            IncreasePomodoroCountCommand = new RelayCommand(_ => PomodorosUntilLongBreak = Math.Min(8, PomodorosUntilLongBreak + 1));
            DecreasePomodoroCountCommand = new RelayCommand(_ => PomodorosUntilLongBreak = Math.Max(2, PomodorosUntilLongBreak - 1));
            
            // 加载保存的设置
            LoadSettings();
        }

        /// <summary>
        /// 加载设置
        /// </summary>
        private void LoadSettings()
        {
            var settings = SettingsService.LoadSettings();
            _workDuration = settings.WorkDuration;
            _shortBreakDuration = settings.ShortBreakDuration;
            _longBreakDuration = settings.LongBreakDuration;
            _pomodorosUntilLongBreak = settings.PomodorosUntilLongBreak;
            
            // 通知所有属性变更
            OnPropertyChanged(nameof(WorkDuration));
            OnPropertyChanged(nameof(ShortBreakDuration));
            OnPropertyChanged(nameof(LongBreakDuration));
            OnPropertyChanged(nameof(PomodorosUntilLongBreak));
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        private void SaveSettings()
        {
            var settings = new PomodoroSettings
            {
                WorkDuration = WorkDuration,
                ShortBreakDuration = ShortBreakDuration,
                LongBreakDuration = LongBreakDuration,
                PomodorosUntilLongBreak = PomodorosUntilLongBreak
            };
            SettingsService.SaveSettings(settings);
        }

        /// <summary>
        /// 重置为默认值
        /// </summary>
        private void ResetToDefault()
        {
            WorkDuration = 25;
            ShortBreakDuration = 5;
            LongBreakDuration = 15;
            PomodorosUntilLongBreak = 4;
        }

        /// <summary>
        /// 触发设置变更事件
        /// </summary>
        private void OnSettingsChanged()
        {
            // 保存设置到文件
            SaveSettings();
            
            // 触发设置变更事件
            SettingsChanged?.Invoke(this, new SettingsChangedEventArgs
            {
                WorkDuration = WorkDuration,
                ShortBreakDuration = ShortBreakDuration,
                LongBreakDuration = LongBreakDuration,
                PomodorosUntilLongBreak = PomodorosUntilLongBreak
            });
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// 设置变更事件参数
    /// </summary>
    public class SettingsChangedEventArgs : EventArgs
    {
        public int WorkDuration { get; set; }
        public int ShortBreakDuration { get; set; }
        public int LongBreakDuration { get; set; }
        public int PomodorosUntilLongBreak { get; set; }
    }

    /// <summary>
    /// 简单的命令实现
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;

        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        public void Execute(object? parameter)
        {
            _execute(parameter);
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}