using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace LuaInterpreter_WPF
{
    public class InputHandler : INotifyPropertyChanged
    {
        private static volatile InputHandler instance;
        private static object syncRoot = new Object();

        public event PropertyChangedEventHandler PropertyChanged;

        private InputHandler()
        {
        }

        public static InputHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new InputHandler();
                    }
                }

                return instance;
            }
        }

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string _Text;
        public string Text
        {
            get
            {
                return _Text;
            }
            set
            {
                if (value != _Text)
                {
                    _Text = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public async void LoadInput(string filePath)
        {
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    Text = await sr.ReadToEndAsync();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.LogError("Loading Input : " + ex.Message);
            }
        }

        public async void SaveInput(string filePath)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    await sw.WriteAsync(Text);
                    await sw.FlushAsync();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.LogError("Saving Input : " + ex.Message);
            }
        }
    }
}
