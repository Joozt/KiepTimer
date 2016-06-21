using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace KiepTimer
{
    public partial class Notification : Window
    {
        private const int KEY_CODE_ESCAPE = 27;
        private const int KEY_CODE_Q = 81;
        private DispatcherTimer timer = new DispatcherTimer();
        private MediaPlayer showNotificationPlayer = new MediaPlayer();
        private MediaPlayer hideNotificationPlayer = new MediaPlayer();
        private bool playSound;
        private bool noStealFocus;

        public Notification()
        {
            InitializeComponent();

            InitializeNotificationSounds();
            timer.Tick += Timer_Tick;
        }

        private void InitializeNotificationSounds()
        {
            if (!File.Exists("SoundHide.wav"))
            {
                FileStream soundHideOutputFile = new FileStream("SoundHide.wav", FileMode.Create, FileAccess.Write);
                Properties.Resources.SoundHide.CopyTo(soundHideOutputFile);
                soundHideOutputFile.Close();
            }

            if (!File.Exists("SoundShow.wav"))
            {
                FileStream soundShowOutputFile = new FileStream("SoundShow.wav", FileMode.Create, FileAccess.Write);
                Properties.Resources.SoundShow.CopyTo(soundShowOutputFile);
                soundShowOutputFile.Close();
            }

            showNotificationPlayer.Open(new Uri("SoundShow.wav", UriKind.Relative));
            hideNotificationPlayer.Open(new Uri("SoundHide.wav", UriKind.Relative));
        }

        public void StartTimer(TimeSpan interval, string titleText, Color titleColor, int titleFontSize, string subtitleText, Color subtitleColor, int subtitleFontSize, bool playSound, bool noStealFocus)
        {
            textblockTitle.Text = titleText;
            textblockTitle.Foreground = new SolidColorBrush(titleColor);
            textblockTitle.FontSize = titleFontSize;
            textblockSubtitle.Text = subtitleText;
            textblockSubtitle.Foreground = new SolidColorBrush(subtitleColor);
            textblockSubtitle.FontSize = subtitleFontSize;

            this.noStealFocus = noStealFocus;
            this.playSound = playSound;
            if (playSound)
            {
                hideNotificationPlayer.Position = new TimeSpan(0);
                hideNotificationPlayer.Play();
            }

            timer.Interval = interval;
            timer.Start();
        }

        public void StopTimer()
        {
            timer.Stop();
            HideNotification();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (playSound)
            {
                showNotificationPlayer.Position = new TimeSpan(0);
                showNotificationPlayer.Play();
            }
            timer.Stop();

            if (noStealFocus)
            {
                ShowActivated = false;
                Topmost = true;
                WindowState = WindowState.Normal;
                Width = SystemParameters.PrimaryScreenWidth;
                Height = SystemParameters.PrimaryScreenHeight;
                Top = 0;
                Left = 0;
            }

            ShowNotification();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (playSound)
            {
                hideNotificationPlayer.Position = new TimeSpan(0);
                hideNotificationPlayer.Play();
            }
            HideNotification();
            timer.Start();
        }

        private void LowLevelKeyboardHookEvent(int keycode)
        {
            Console.Out.WriteLine(keycode);

            if (keycode == KEY_CODE_ESCAPE)
            {
                if (playSound)
                {
                    hideNotificationPlayer.Position = new TimeSpan(0);
                    hideNotificationPlayer.Play();
                }
                HideNotification();
                timer.Start();
            }
            if (keycode == KEY_CODE_Q)
            {
                Application.Current.Shutdown();
            }
        }

        private void ShowNotification()
        {
            LowLevelKeyboardHook.Instance.SetBlockedKeys(new List<int>() { KEY_CODE_ESCAPE, KEY_CODE_Q });
            LowLevelKeyboardHook.Instance.KeyboardHookEvent += LowLevelKeyboardHookEvent;
            Show();
        }

        private void HideNotification()
        {
            LowLevelKeyboardHook.Instance.SetBlockedKeys(null);
            LowLevelKeyboardHook.Instance.KeyboardHookEvent -= LowLevelKeyboardHookEvent;
            Hide();
        }
    }
}
