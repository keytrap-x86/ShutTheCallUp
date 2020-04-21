using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;

namespace ShutTheCallUp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly GlobalKeyboardHook _globalKeyboardHook;


        public MainWindow()
        {
            InitializeComponent();
            _globalKeyboardHook = new GlobalKeyboardHook(new[] { Keys.Oem7 });
            _globalKeyboardHook.KeyboardPressed += OnKeyPressed;
        }

        private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            if (e.KeyboardState != GlobalKeyboardHook.KeyboardState.KeyDown) return;

            var proc = Process.GetProcessesByName("ModernWarfare");
            if (proc.Length < 1)
                return;

            var isMuted = VolumeMixer.GetApplicationMute(proc[0].Id);
            if (isMuted.HasValue && isMuted.Value)
                VolumeMixer.SetApplicationMute(proc[0].Id, false);
            else
            {
                VolumeMixer.SetApplicationMute(proc[0].Id, true);
            }

            myNotifyIcon.ToolTipText = (!isMuted.Value).ToString();
        }

        private void MyNotifyIcon_OnTrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            _globalKeyboardHook?.Dispose();
            Environment.Exit(0);
        }
    }
}
