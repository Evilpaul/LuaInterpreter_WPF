using System;
using System.ComponentModel;
using System.IO;

namespace LuaInterpreter_WPF
{
    public class InputHandler : ObservableObject
    {
        private static volatile InputHandler instance;
        private static object syncRoot = new object();

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
                    OnPropertyChanged(nameof(Text));
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
