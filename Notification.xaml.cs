using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace KiepTimer
{
    public partial class Notification : Window
    {
        private System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        private System.Windows.Media.MediaPlayer showNotificationPlayer = new System.Windows.Media.MediaPlayer();
        private System.Windows.Media.MediaPlayer hideNotificationPlayer = new System.Windows.Media.MediaPlayer();
        private bool playSound;

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

        public void StartTimer(TimeSpan interval, string titleText, Color titleColor, int titleFontSize, string subtitleText, Color subtitleColor, int subtitleFontSize, bool playSound)
        {
            textblockTitle.Text = titleText;
            textblockTitle.Foreground = new SolidColorBrush(titleColor);
            textblockTitle.FontSize = titleFontSize;
            textblockSubtitle.Text = subtitleText;
            textblockSubtitle.Foreground = new SolidColorBrush(subtitleColor);
            textblockSubtitle.FontSize = subtitleFontSize;

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
            Hide();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (playSound)
            {
                showNotificationPlayer.Position = new TimeSpan(0);
                showNotificationPlayer.Play();
            }
            timer.Stop();
            Show();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (playSound)
            {
                hideNotificationPlayer.Position = new TimeSpan(0);
                hideNotificationPlayer.Play();
            }
            Hide();
            timer.Start();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (playSound)
                {
                    hideNotificationPlayer.Position = new TimeSpan(0);
                    hideNotificationPlayer.Play();
                }
                Hide();
                timer.Start();
            }
            if (e.Key == Key.Q)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
