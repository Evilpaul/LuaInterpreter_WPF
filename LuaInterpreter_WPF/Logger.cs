using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace LuaInterpreter_WPF
{
    public class Logger
    {
        private static volatile Logger instance;
        private static object syncRoot = new object();

        public class LogItem
        {
            public string Message { get; }
            public Brush MessageColor { get; }

            public LogItem(string message, Brush color)
            {
                Message = message;
                MessageColor = color;
            }
        }

        public ObservableCollection<LogItem> logList { get; }

        private void AddItem(LogItem li)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                logList.Add(li);
            });
        }

        private Logger()
        {
            logList = new ObservableCollection<LogItem>();
        }

        public static Logger Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Logger();
                    }
                }

                return instance;
            }
        }

        public void ClearLog() => logList.Clear();

        public void LogInfo(string info) => AddItem(new LogItem(info, Brushes.Black));
        public void LogExecutionInfo(long ticks, string info) => AddItem(new LogItem($"{ticks} | {info}", Brushes.Gray));
        public void LogExecutionResult(string info) => AddItem(new LogItem($"Return value : {info}", Brushes.Green));
        public void LogError(string error) => AddItem(new LogItem($"ERROR | {error}", Brushes.Red));
        public void LogWarning(string warning) => AddItem(new LogItem($"WARNING | {warning}", Brushes.Orange));

        public async void SaveLog(string filePath)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    foreach (LogItem line in logList)
                    {
                        await sw.WriteLineAsync(line.Message);
                    }

                    await sw.FlushAsync();
                }
            }
            catch (Exception ex)
            {
                LogError($"Saving Log : {ex.Message}");
            }
        }
    }
}
