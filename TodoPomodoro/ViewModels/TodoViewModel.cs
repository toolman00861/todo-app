using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TodoPomodoro.Models;

namespace TodoPomodoro.ViewModels
{
    /// <summary>
    /// 待办事项视图模型
    /// </summary>
    public class TodoViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<TodoItem> _todoItems;
        private TodoItem? _selectedTodoItem;
        private string _newTodoTitle = string.Empty;

        /// <summary>
        /// 待办事项集合
        /// </summary>
        public ObservableCollection<TodoItem> TodoItems
        {
            get => _todoItems;
            set
            {
                _todoItems = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 当前选中的待办事项
        /// </summary>
        public TodoItem? SelectedTodoItem
        {
            get => _selectedTodoItem;
            set
            {
                _selectedTodoItem = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsItemSelected));
            }
        }

        /// <summary>
        /// 新待办事项的标题
        /// </summary>
        public string NewTodoTitle
        {
            get => _newTodoTitle;
            set
            {
                _newTodoTitle = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 是否有选中的待办事项
        /// </summary>
        public bool IsItemSelected => SelectedTodoItem != null;

        /// <summary>
        /// 添加待办事项命令
        /// </summary>
        public ICommand AddTodoCommand { get; }

        /// <summary>
        /// 删除待办事项命令
        /// </summary>
        public ICommand DeleteTodoCommand { get; }

        /// <summary>
        /// 切换待办事项完成状态命令
        /// </summary>
        public ICommand ToggleCompletionCommand { get; }

        /// <summary>
        /// 待办事项添加事件
        /// </summary>
        public event EventHandler<TodoItem> TodoItemAdded;

        /// <summary>
        /// 待办事项删除事件
        /// </summary>
        public event EventHandler<TodoItem> TodoItemDeleted;

        /// <summary>
        /// 待办事项状态变更事件
        /// </summary>
        public event EventHandler<TodoItem> TodoItemStatusChanged;

        /// <summary>
        /// 构造函数
        /// </summary>
        public TodoViewModel()
        {
            _todoItems = new ObservableCollection<TodoItem>();
            
            // 添加一些示例数据
            _todoItems.Add(new TodoItem { Title = "完成待办应用", Description = "实现待办事项和番茄钟功能", Priority = Priority.High });
            _todoItems.Add(new TodoItem { Title = "学习WPF", Description = "掌握MVVM模式", Priority = Priority.Medium });
            _todoItems.Add(new TodoItem { Title = "锻炼身体", Description = "每天30分钟", Priority = Priority.Low });

            // 初始化命令
            AddTodoCommand = new RelayCommand(AddTodo, CanAddTodo);
            DeleteTodoCommand = new RelayCommand(DeleteTodo, CanDeleteTodo);
            ToggleCompletionCommand = new RelayCommand(ToggleCompletion, CanToggleCompletion);
        }

        private bool CanToggleCompletion(object? parameter)
        {
            return SelectedTodoItem != null;
        }

        private void ToggleCompletion(object? parameter)
        {
            if (SelectedTodoItem != null)
            {
                SelectedTodoItem.IsCompleted = !SelectedTodoItem.IsCompleted;
                OnPropertyChanged(nameof(TodoItems));
                
                // 触发状态变更事件
                TodoItemStatusChanged?.Invoke(this, SelectedTodoItem);
            }
        }

        private bool CanDeleteTodo(object? parameter)
        {
            return SelectedTodoItem != null;
        }

        private void DeleteTodo(object? parameter)
        {
            if (SelectedTodoItem != null)
            {
                var itemToDelete = SelectedTodoItem;
                TodoItems.Remove(itemToDelete);
                SelectedTodoItem = null;
                
                // 触发删除事件
                TodoItemDeleted?.Invoke(this, itemToDelete);
            }
        }

        private bool CanAddTodo(object? parameter)
        {
            return !string.IsNullOrWhiteSpace(NewTodoTitle);
        }

        private void AddTodo(object? parameter)
        {
            if (!string.IsNullOrWhiteSpace(NewTodoTitle))
            {
                var newTodo = new TodoItem
                {
                    Title = NewTodoTitle,
                    CreatedAt = DateTime.Now
                };

                TodoItems.Add(newTodo);
                
                // 添加后选中新项目
                SelectedTodoItem = newTodo;
                
                // 清空输入框
                NewTodoTitle = string.Empty;
                
                // 触发动画效果（通过事件通知视图）
                TodoItemAdded?.Invoke(this, newTodo);
            }
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

    /// <summary>
    /// 命令实现类
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?> _canExecute;

        /// <summary>
        /// 构造函数
        /// </summary>
        public RelayCommand(Action<object?> execute, Predicate<object?> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// 判断命令是否可执行
        /// </summary>
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        public void Execute(object? parameter)
        {
            _execute(parameter);
        }

        /// <summary>
        /// 可执行状态变更事件
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}