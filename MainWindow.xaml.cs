using Microsoft.Win32;
using System;
using System.Reflection;
using System.Windows;

namespace KiepTimer
{
    public partial class MainWindow : Window
    {
        Notification notification = new Notification();

        public MainWindow()
        {
            InitializeComponent();

            Title = "KiepTimer v" + Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".0.0", "");
            taskbarIcon.ToolTipText = Title;
            notification.Title = Title;

            interval.Focus();
            ReadSettings();
            ReadCommandLine();
        }

        private void ReadSettings()
        {
            interval.Value = Properties.Settings.Default.Interval;
            textTitle.Text = Properties.Settings.Default.TitleText;
            colorPickerTitle.SelectedColor = Properties.Settings.Default.TitleColor;
            fontSizeTitle.Value = Properties.Settings.Default.TitleFontSize;
            textSubtitle.Text = Properties.Settings.Default.SubtitleText;
            colorPickerSubtitle.SelectedColor = Properties.Settings.Default.SubtitleColor;
            fontSizeSubtitle.Value = Properties.Settings.Default.SubtitleFontSize;
            cbSound.IsChecked = Properties.Settings.Default.PlaySound;
            cbNoStealFocus.IsChecked = Properties.Settings.Default.NoStealFocus;
            cbAutoStart.IsChecked = IsAutoStart();

            if (string.IsNullOrEmpty(textTitle.Text))
            {
                textTitle.Text = Properties.Resources.DefaultTitle;
            }
            if (string.IsNullOrEmpty(textSubtitle.Text))
            {
                textSubtitle.Text = Properties.Resources.DefaultSubtitle;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();

            Visibility = Visibility.Hidden;
            ShowInTaskbar = false;

            notification.StartTimer(interval.Value.GetValueOrDefault(),
                textTitle.Text, colorPickerTitle.SelectedColor.GetValueOrDefault(), fontSizeTitle.Value.GetValueOrDefault(),
                textSubtitle.Text, colorPickerSubtitle.SelectedColor.GetValueOrDefault(), fontSizeSubtitle.Value.GetValueOrDefault(),
                cbSound.IsChecked.GetValueOrDefault(), cbNoStealFocus.IsChecked.GetValueOrDefault());
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.Interval = interval.Value.GetValueOrDefault();
            Properties.Settings.Default.TitleText = textTitle.Text;
            Properties.Settings.Default.TitleColor = colorPickerTitle.SelectedColor.GetValueOrDefault();
            Properties.Settings.Default.TitleFontSize = fontSizeTitle.Value.GetValueOrDefault();
            Properties.Settings.Default.SubtitleText = textSubtitle.Text;
            Properties.Settings.Default.SubtitleColor = colorPickerSubtitle.SelectedColor.GetValueOrDefault();
            Properties.Settings.Default.SubtitleFontSize = fontSizeSubtitle.Value.GetValueOrDefault();
            Properties.Settings.Default.PlaySound = cbSound.IsChecked.GetValueOrDefault();
            Properties.Settings.Default.NoStealFocus = cbNoStealFocus.IsChecked.GetValueOrDefault();
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
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (cbAutoStart.IsChecked.GetValueOrDefault())
            {
                registryKey.SetValue("KiepTimer", "\"" + Assembly.GetExecutingAssembly().Location + "\" autostart");
            }
            else
            {
                registryKey.DeleteValue("KiepTimer");
            }
        }

        private bool IsAutoStart()
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
            return registryKey.GetValue("KiepTimer") != null;
        }
    }
}
