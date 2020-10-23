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

namespace ClientApplication.ShoppingCartScreen
{
    /// <summary>
    /// Interaction logic for ShoppingCartUC.xaml
    /// </summary>
    public partial class ShoppingCartUC : UserControl
    {
        private MainWindow mainWindow;
        public ShoppingCartUC(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();

            InCart = new List<InCartProduct>();

            //    InCartProductsList.ItemsSource = InCart;

            InCart = mainWindow.GetInCartProducts();
            if (InCart.Count > 0)
                InCartProductsList.ItemsSource = InCart;
        }

        public List<InCartProduct> InCart { get; set; }

        private void Button_ContinueShopping(object sender, RoutedEventArgs e)
        {
            mainWindow.ChangeView("MainProduct");
        }

        private void Button_ProceedPurchase(object sender, RoutedEventArgs e)
        {
            mainWindow.ChangeView("PurchaseCheckout");
        }

        private void Button_Remove(object sender, RoutedEventArgs e)
        {
            string p = ((Button)sender).Tag.ToString();
            foreach (InCartProduct product in InCart)
            {
                if (product.ProductName.Equals(p))
                {
                    InCart.Remove(product);
                    InCartProductsList.ItemsSource = null;
                    InCartProductsList.ItemsSource = InCart;
                    return;
                }
            }
        }
    }
}
