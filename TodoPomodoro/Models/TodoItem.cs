using System;

namespace TodoPomodoro.Models
{
    /// <summary>
    /// 表示一个待办事项
    /// </summary>
    public class TodoItem
    {
        /// <summary>
        /// 待办事项的唯一标识符
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// 待办事项的标题
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 待办事项的描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 待办事项是否已完成
        /// </summary>
        public bool IsCompleted { get; set; } = false;

        /// <summary>
        /// 待办事项的创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// 待办事项的截止时间（可选）
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// 待办事项的优先级
        /// </summary>
        public Priority Priority { get; set; } = Priority.Medium;

        /// <summary>
        /// 待办事项的预计番茄钟数量
        /// </summary>
        public int EstimatedPomodoros { get; set; } = 1;

        /// <summary>
        /// 待办事项已完成的番茄钟数量
        /// </summary>
        public int CompletedPomodoros { get; set; } = 0;
    }

    /// <summary>
    /// 表示待办事项的优先级
    /// </summary>
    public enum Priority
    {
        Low,
        Medium,
        High
    }
}