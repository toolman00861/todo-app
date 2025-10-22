using System;
using System.Windows.Threading;

namespace TodoPomodoro.Models
{
    /// <summary>
    /// 表示番茄钟的状态
    /// </summary>
    public enum PomodoroState
    {
        Ready,
        WorkInProgress,
        ShortBreak,
        LongBreak,
        Paused
    }

    /// <summary>
    /// 番茄钟计时器类
    /// </summary>
    public class PomodoroTimer
    {
        private DispatcherTimer _timer;
        private DateTime _endTime;
        private int _completedPomodoros = 0;
        private int _pomodorosUntilLongBreak = 4;

        /// <summary>
        /// 工作时间长度（分钟）
        /// </summary>
        public int WorkDuration { get; set; } = 25;

        /// <summary>
        /// 短休息时间长度（分钟）
        /// </summary>
        public int ShortBreakDuration { get; set; } = 5;

        /// <summary>
        /// 长休息时间长度（分钟）
        /// </summary>
        public int LongBreakDuration { get; set; } = 15;

        /// <summary>
        /// 当前番茄钟状态
        /// </summary>
        public PomodoroState State { get; private set; } = PomodoroState.Ready;

        /// <summary>
        /// 剩余时间（秒）
        /// </summary>
        public int RemainingSeconds { get; private set; }

        /// <summary>
        /// 已完成的番茄钟数量
        /// </summary>
        public int CompletedPomodoros => _completedPomodoros;

        /// <summary>
        /// 当前关联的待办事项（可选）
        /// </summary>
        public TodoItem? CurrentTodoItem { get; set; }

        /// <summary>
        /// 计时器状态变化事件
        /// </summary>
        public event EventHandler? TimerTick;

        /// <summary>
        /// 番茄钟完成事件
        /// </summary>
        public event EventHandler? PomodoroCompleted;

        /// <summary>
        /// 休息时间完成事件
        /// </summary>
        public event EventHandler? BreakCompleted;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PomodoroTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            RemainingSeconds = (int)(_endTime - DateTime.Now).TotalSeconds;
            
            if (RemainingSeconds <= 0)
            {
                RemainingSeconds = 0;
                
                switch (State)
                {
                    case PomodoroState.WorkInProgress:
                        _completedPomodoros++;
                        if (CurrentTodoItem != null)
                        {
                            CurrentTodoItem.CompletedPomodoros++;
                        }
                        PomodoroCompleted?.Invoke(this, EventArgs.Empty);
                        
                        if (_completedPomodoros % _pomodorosUntilLongBreak == 0)
                        {
                            StartLongBreak();
                        }
                        else
                        {
                            StartShortBreak();
                        }
                        break;
                        
                    case PomodoroState.ShortBreak:
                    case PomodoroState.LongBreak:
                        BreakCompleted?.Invoke(this, EventArgs.Empty);
                        State = PomodoroState.Ready;
                        _timer.Stop();
                        break;
                }
            }
            
            TimerTick?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 开始工作计时
        /// </summary>
        public void StartWork()
        {
            State = PomodoroState.WorkInProgress;
            _endTime = DateTime.Now.AddMinutes(WorkDuration);
            RemainingSeconds = WorkDuration * 60;
            _timer.Start();
            TimerTick?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 开始短休息
        /// </summary>
        public void StartShortBreak()
        {
            State = PomodoroState.ShortBreak;
            _endTime = DateTime.Now.AddMinutes(ShortBreakDuration);
            RemainingSeconds = ShortBreakDuration * 60;
            _timer.Start();
            TimerTick?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 开始长休息
        /// </summary>
        public void StartLongBreak()
        {
            State = PomodoroState.LongBreak;
            _endTime = DateTime.Now.AddMinutes(LongBreakDuration);
            RemainingSeconds = LongBreakDuration * 60;
            _timer.Start();
            TimerTick?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 暂停计时器
        /// </summary>
        public void Pause()
        {
            if (State == PomodoroState.WorkInProgress || 
                State == PomodoroState.ShortBreak || 
                State == PomodoroState.LongBreak)
            {
                _timer.Stop();
                State = PomodoroState.Paused;
                TimerTick?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 恢复计时器
        /// </summary>
        public void Resume()
        {
            if (State == PomodoroState.Paused)
            {
                _timer.Start();
                State = PomodoroState.WorkInProgress;
                TimerTick?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 重置计时器
        /// </summary>
        public void Reset()
        {
            _timer.Stop();
            State = PomodoroState.Ready;
            RemainingSeconds = 0;
            TimerTick?.Invoke(this, EventArgs.Empty);
        }
    }
}