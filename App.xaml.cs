using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

namespace KiepTimer
{
    public partial class App : Application
    {
        App()
        {
#if DEBUG
            // Uncomment to reset all settings
            //KiepTimer.Properties.Settings.Default.Reset();

            // Uncomment to always use Dutch resources
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("nl");
#endif
        }
    }
}
