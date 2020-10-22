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

namespace ClientApplication.PreviousOrderScreen
{
    /// <summary>
    /// Interaction logic for PreviousOrderScreenUC.xaml
    /// </summary>
    public partial class PreviousOrderScreenUC : UserControl
    {
        private MainWindow mainWindow;
        public PreviousOrderScreenUC(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();

            SetProductsListSource();
        }

        public void SetProductsListSource()
        {
            var inCartProducts = mainWindow.GetInCartProducts();
            if (inCartProducts.Count > 0)
                InCartProductsList.ItemsSource = inCartProducts;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
           

        }
    }
}
