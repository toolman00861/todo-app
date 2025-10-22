using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TodoPomodoro.Models;

namespace TodoPomodoro.ViewModels
{
    /// <summary>
    /// 番茄钟视图模型
    /// </summary>
    public class PomodoroViewModel : INotifyPropertyChanged
    {
        private readonly PomodoroTimer _timer;
        private string _timeDisplay = "25:00";
        private string _stateDisplay = "准备就绪";
        private string _pomodoroCountDisplay = "0";
        private PomodoroState _previousState = PomodoroState.Ready;

        /// <summary>
        /// 番茄钟计时器
        /// </summary>
        public PomodoroTimer Timer => _timer;

        /// <summary>
        /// 时间显示
        /// </summary>
        public string TimeDisplay
        {
            get => _timeDisplay;
            set
            {
                _timeDisplay = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 状态显示
        /// </summary>
        public string StateDisplay
        {
            get => _stateDisplay;
            set
            {
                _stateDisplay = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 番茄钟计数显示
        /// </summary>
        public string PomodoroCountDisplay
        {
            get => _pomodoroCountDisplay;
            set
            {
                _pomodoroCountDisplay = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 是否可以开始
        /// </summary>
        public bool CanStart => _timer.State == PomodoroState.Ready;

        /// <summary>
        /// 是否可以暂停
        /// </summary>
        public bool CanPause => _timer.State == PomodoroState.WorkInProgress || 
                               _timer.State == PomodoroState.ShortBreak || 
                               _timer.State == PomodoroState.LongBreak;

        /// <summary>
        /// 是否可以恢复
        /// </summary>
        public bool CanResume => _timer.State == PomodoroState.Paused;

        /// <summary>
        /// 是否可以重置
        /// </summary>
        public bool CanReset => _timer.State != PomodoroState.Ready;

        /// <summary>
        /// 开始命令
        /// </summary>
        public ICommand StartCommand { get; }

        /// <summary>
        /// 暂停命令
        /// </summary>
        public ICommand PauseCommand { get; }

        /// <summary>
        /// 重置命令
        /// </summary>
        public ICommand ResetCommand { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PomodoroViewModel()
        {
            _timer = new PomodoroTimer();
            _timer.TimerTick += Timer_TimerTick;
            _timer.PomodoroCompleted += Timer_PomodoroCompleted;
            _timer.BreakCompleted += Timer_BreakCompleted;

            StartCommand = new RelayCommand(_ => Start(), _ => CanStart);
            PauseCommand = new RelayCommand(_ => Pause(), _ => CanPause);
            ResetCommand = new RelayCommand(_ => Reset(), _ => CanReset);

            UpdateTimeDisplay();
            UpdateStateDisplay();
            UpdatePomodoroCountDisplay();
        }

        private void Timer_BreakCompleted(object? sender, EventArgs e)
        {
            UpdateTimeDisplay();
            UpdateStateDisplay();
            UpdatePomodoroCountDisplay();
            UpdateCommandsCanExecute();
        }

        private void Timer_PomodoroCompleted(object? sender, EventArgs e)
        {
            UpdateTimeDisplay();
            UpdateStateDisplay();
            UpdatePomodoroCountDisplay();
            UpdateCommandsCanExecute();
        }

        private void Timer_TimerTick(object? sender, EventArgs e)
        {
            UpdateTimeDisplay();
            UpdateStateDisplay();
            UpdatePomodoroCountDisplay();
            UpdateCommandsCanExecute();
        }

        private void UpdateCommandsCanExecute()
        {
            OnPropertyChanged(nameof(CanStart));
            OnPropertyChanged(nameof(CanPause));
            OnPropertyChanged(nameof(CanReset));
        }

        private void UpdatePomodoroCountDisplay()
        {
            PomodoroCountDisplay = _timer.CompletedPomodoros.ToString();
        }

        private void UpdateStateDisplay()
        {
            StateDisplay = _timer.State switch
            {
                PomodoroState.Ready => "准备就绪",
                PomodoroState.WorkInProgress => "工作中",
                PomodoroState.ShortBreak => "短休息",
                PomodoroState.LongBreak => "长休息",
                PomodoroState.Paused => "已暂停",
                _ => "未知状态"
            };
        }

        private void UpdateTimeDisplay()
        {
            if (_timer.State == PomodoroState.Ready)
            {
                // 显示默认工作时间
                TimeDisplay = $"{_timer.WorkDuration:D2}:00";
            }
            else
            {
                int minutes = _timer.RemainingSeconds / 60;
                int seconds = _timer.RemainingSeconds % 60;
                TimeDisplay = $"{minutes:D2}:{seconds:D2}";
            }
        }

        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            if (_timer.State == PomodoroState.Ready)
            {
                _timer.StartWork();
            }
            else if (_timer.State == PomodoroState.Paused)
            {
                Resume();
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            _previousState = _timer.State;
            _timer.Pause();
        }

        /// <summary>
        /// 恢复
        /// </summary>
        public void Resume()
        {
            if (_timer.State == PomodoroState.Paused)
            {
                _timer.Resume();
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            _timer.Reset();
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