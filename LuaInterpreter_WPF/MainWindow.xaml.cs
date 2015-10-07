using Microsoft.Win32;
using System.Windows;

namespace LuaInterpreter_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Worker worker;

        public MainWindow()
        {
            InitializeComponent();

            worker = new Worker();
        }

        private void OpenInput_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = "*.lua";
            ofd.Filter = "Lua Files (*.lua)|*.lua";

            bool? result = ofd.ShowDialog();

            if (result == true)
            {
                InputHandler.Instance.LoadInput(ofd.FileName);
            }
        }

        private void SaveInput_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ".lua";
            sfd.Filter = "Lua Files (*.lua)|*.lua";

            bool? result = sfd.ShowDialog();

            if (result == true)
            {
                InputHandler.Instance.SaveInput(sfd.FileName);
            }
        }

        private void SaveLog_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ".log";
            sfd.Filter = "Log Files (*.log)|*.log";

            bool? result = sfd.ShowDialog();

            if (result == true)
            {
                Logger.Instance.SaveLog(sfd.FileName);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            worker.doScript();
        }
    }
}
