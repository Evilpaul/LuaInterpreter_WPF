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
        private static object syncRoot = new Object();

        public class LogItem
        {
            public string Message { get; set; }
            public Brush MessageColor { get; set; }
        }

        public ObservableCollection<LogItem> logList { get; set; }

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

        public void ClearLog()
        {
            logList.Clear();
        }

        public void LogInfo(string info)
        {
            LogItem li = new LogItem();
            li.Message = info;
            li.MessageColor = Brushes.Black;
            AddItem(li);
        }

        public void LogExecutionInfo(long ticks, string info)
        {
            LogItem li = new LogItem();
            li.Message = ticks + " | " + info;
            li.MessageColor = Brushes.Gray;
            AddItem(li);
        }

        public void LogExecutionResult(string info)
        {
            LogItem li = new LogItem();
            li.Message = "Return value : " + info;
            li.MessageColor = Brushes.Green;
            AddItem(li);
        }

        public void LogError(string error)
        {
            LogItem li = new LogItem();
            li.Message = "ERROR | " + error;
            li.MessageColor = Brushes.Red;
            AddItem(li);
        }

        public void LogWarning(string warning)
        {
            LogItem li = new LogItem();
            li.Message = "WARNING | " + warning;
            li.MessageColor = Brushes.Orange;
            AddItem(li);
        }

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
                LogError("Saving Log : " + ex.Message);
            }
        }
    }
}
