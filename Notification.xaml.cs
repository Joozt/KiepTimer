using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace KiepTimer
{
    public partial class Notification : Window
    {
        private System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        private System.Media.SoundPlayer showNotificationSound = new System.Media.SoundPlayer(Properties.Resources.VideoRecord);
        private System.Media.SoundPlayer hideNotificationSound = new System.Media.SoundPlayer(Properties.Resources.VideoStop);
        private bool playSound;

        public Notification()
        {
            InitializeComponent();
            timer.Tick += Timer_Tick;
        }

        public void StartTimer(TimeSpan interval, string text, bool playSound, Color color, int fontSize)
        {
            this.playSound = playSound;
            textLabel.Content = text;
            textLabel.Foreground = new SolidColorBrush(color);
            textLabel.FontSize = fontSize;

            if (playSound)
            {
                hideNotificationSound.Play();
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
                showNotificationSound.Play();
            }
            timer.Stop();
            Show();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (playSound)
            {
                hideNotificationSound.Play();
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
                    hideNotificationSound.Play();
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
