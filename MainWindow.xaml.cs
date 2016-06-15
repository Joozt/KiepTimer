using System;
using System.IO;
using System.Windows;

namespace KiepTimer
{
    public partial class MainWindow : Window
    {
        Notification notification = new Notification();

        public MainWindow()
        {
            InitializeComponent();

            Title = "KiepTimer v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".0.0", "");
            taskbarIcon.ToolTipText = Title;
            notification.Title = Title;

            interval.Focus();
            ReadSettings();
            ReadCommandLine();
        }

        private void ReadSettings()
        {
            interval.Value = Properties.Settings.Default.Interval;
            text.Text = Properties.Settings.Default.Text;
            colorPicker.SelectedColor = Properties.Settings.Default.Color;
            fontSize.Value = Properties.Settings.Default.Size;
            cbSound.IsChecked = Properties.Settings.Default.PlaySound;
            cbAutoStart.IsChecked = IsAutoStart();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();

            Visibility = Visibility.Hidden;
            ShowInTaskbar = false;

            notification.StartTimer(interval.Value.GetValueOrDefault(), text.Text, cbSound.IsChecked.GetValueOrDefault(), colorPicker.SelectedColor.GetValueOrDefault(), fontSize.Value.GetValueOrDefault());
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.Interval = interval.Value.GetValueOrDefault();
            Properties.Settings.Default.Text = text.Text;
            Properties.Settings.Default.PlaySound = cbSound.IsChecked.GetValueOrDefault();
            Properties.Settings.Default.Color = colorPicker.SelectedColor.GetValueOrDefault();
            Properties.Settings.Default.Size = fontSize.Value.GetValueOrDefault();
            Properties.Settings.Default.Save();
        }

        private void TaskbarIcon_TrayMouseDown(object sender, RoutedEventArgs e)
        {
            notification.StopTimer();

            Visibility = Visibility.Visible;
            ShowInTaskbar = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ReadCommandLine()
        {
            string[] args = Environment.GetCommandLineArgs();
            foreach (string arg in args)
            {
                if (arg.Contains("autostart"))
                {
                    button_Click(null, null);
                }
            }
        }

        private void cbAutoStart_CheckedChanged(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (cbAutoStart.IsChecked.GetValueOrDefault())
            {
                registryKey.SetValue("KiepTimer", "\"" + System.Reflection.Assembly.GetExecutingAssembly().Location + "\" autostart");
            }
            else
            {
                registryKey.DeleteValue("KiepTimer");
            }
        }

        private bool IsAutoStart()
        {
            Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
            return registryKey.GetValue("KiepTimer") != null;
        }
    }
}
