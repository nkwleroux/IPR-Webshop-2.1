using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientApplication.NoConnectionScreen
{
    /// <summary>
    /// Interaction logic for NoConnectionScreenUC.xaml
    /// </summary>
    public partial class NoConnectionScreenUC : UserControl
    {
        private MainWindow mainWindow;
        public NoConnectionScreenUC(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;

            InitializeComponent();

        }

        private void Button_Reconnect(object sender, RoutedEventArgs e)
        {
            Thread t = new Thread(new ThreadStart(Reconnect));
            t.Start();
        }

        private void Reconnect()
        {
            mainWindow.client.Reconnect();
            if (mainWindow.client.GetClient().Connected)
            {
                this.Dispatcher.Invoke(() =>
                {
                    mainWindow.ChangeView("MainProduct");
                });
            }
        }
    }
}
