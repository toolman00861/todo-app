using System;
using System.IO;
using System.Text.Json;

namespace TodoPomodoro.Services
{
    /// <summary>
    /// 设置数据模型
    /// </summary>
    public class PomodoroSettings
    {
        public int WorkDuration { get; set; } = 25;
        public int ShortBreakDuration { get; set; } = 5;
        public int LongBreakDuration { get; set; } = 15;
        public int PomodorosUntilLongBreak { get; set; } = 4;
    }

    /// <summary>
    /// 设置持久化服务
    /// </summary>
    public class SettingsService
    {
        private static readonly string SettingsFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "TodoPomodoro",
            "settings.json"
        );

        /// <summary>
        /// 保存设置
        /// </summary>
        /// <param name="settings">设置对象</param>
        public static void SaveSettings(PomodoroSettings settings)
        {
            try
            {
                // 确保目录存在
                var directory = Path.GetDirectoryName(SettingsFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // 序列化并保存设置
                var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                File.WriteAllText(SettingsFilePath, json);
            }
            catch (Exception ex)
            {
                // 记录错误但不抛出异常，避免影响应用程序运行
                System.Diagnostics.Debug.WriteLine($"保存设置失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 加载设置
        /// </summary>
        /// <returns>设置对象，如果加载失败则返回默认设置</returns>
        public static PomodoroSettings LoadSettings()
        {
            try
            {
                if (File.Exists(SettingsFilePath))
                {
                    var json = File.ReadAllText(SettingsFilePath);
                    var settings = JsonSerializer.Deserialize<PomodoroSettings>(json);
                    return settings ?? new PomodoroSettings();
                }
            }
            catch (Exception ex)
            {
                // 记录错误但不抛出异常，返回默认设置
                System.Diagnostics.Debug.WriteLine($"加载设置失败: {ex.Message}");
            }

            return new PomodoroSettings();
        }

        /// <summary>
        /// 删除设置文件
        /// </summary>
        public static void DeleteSettings()
        {
            try
            {
                if (File.Exists(SettingsFilePath))
                {
                    File.Delete(SettingsFilePath);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"删除设置失败: {ex.Message}");
            }
        }
    }
}