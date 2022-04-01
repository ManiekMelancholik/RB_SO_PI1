using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RB_SO_PI1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static public MainWindowMV mv;
        public void InitApp(object o, StartupEventArgs e)
        {
            mv = new MainWindowMV(false);
        }
    }
}
