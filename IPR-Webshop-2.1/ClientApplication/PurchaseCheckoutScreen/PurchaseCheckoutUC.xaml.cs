using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientApplication.PurchaseCheckoutScreen
{
    /// <summary>
    /// Interaction logic for PurchaseCheckoutUC.xaml
    /// </summary>
    public partial class PurchaseCheckoutUC : UserControl
    {
        private MainWindow mainWindow;
        public PurchaseCheckoutUC(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
        }

        private void Button_Home(object sender, RoutedEventArgs e)
        {
            mainWindow.ChangeView("MainProduct");
        }
    }
}
