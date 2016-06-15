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
            // Uncomment to always use Dutch resources
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("nl");
        }
    }
}
