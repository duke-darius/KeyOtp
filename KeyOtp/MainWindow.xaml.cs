using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeyOtp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        public IntPtr? LastWindow { get; set; }

        public static LocalSettings Settings { get; set; }

        public MainWindow()
        {
            Application.Current.Exit += Current_Exit;
            KillProcesses();
            Settings = LocalSettings.Load();
            InitializeComponent();
            Visibility = Visibility.Hidden;
            NHotkey.Wpf.HotkeyManager.Current.AddOrReplace("OpenOTP", Key.O, ModifierKeys.Shift | ModifierKeys.Windows, (sender, arg) =>
            {
                ShowActionWindow();
            });
        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            NHotkey.Wpf.HotkeyManager.Current.Remove("OpenOTP");
        }

        private void KillProcesses()
        {
            var procs = Process.GetProcessesByName("KeyOtp");
            var current = Process.GetCurrentProcess();
            foreach(var p in procs.Where(x=> x.Id != current.Id))
            {
                p.Kill(true);
            }


        }

        public void ShowActionWindow()
        {
            if (LastWindow.HasValue)
                return;
            PreAction();

            var win = new OtpWindow();
            win.ShowDialog();

            PostAction();
         
        }

        public void PreAction()
        {
            LastWindow = GetForegroundWindow();
        }

        public void PostAction()
        {
            SetForegroundWindow(LastWindow ?? throw new ArgumentNullException(nameof(LastWindow)));
            LastWindow = null;
        }
    }
}
