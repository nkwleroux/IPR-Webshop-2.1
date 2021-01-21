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

namespace Shared.PurchaseCheckoutScreen
{
    /// <summary>
    /// Interaction logic for PurchaseCheckoutUC.xaml
    /// </summary>
    public partial class PurchaseCheckoutUC : UserControl
    {
        private MainWindow mainWindow;

        /// <summary>
        /// The constructor of PurchaseCheckoutUC
        /// </summary>
        /// <param name="mainWindow">
        /// MainWindow is used to change the view of the application.
        /// </param>
        public PurchaseCheckoutUC(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
        }

        /// <summary>
        /// Used to navigate back to the Home screen.
        /// </summary>
        /// <param name="sender">
        /// The data of the object.
        /// </param>
        /// <param name="e">
        /// The button event argument.
        /// </param>
        private void Button_Home(object sender, RoutedEventArgs e)
        {
            mainWindow.ChangeView("MainProduct");
        }
    }
}
