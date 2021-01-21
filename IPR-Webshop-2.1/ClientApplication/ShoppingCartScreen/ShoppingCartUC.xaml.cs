using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Shared.ShoppingCartScreen
{
    /// <summary>
    /// Interaction logic for ShoppingCartUC.xaml
    /// </summary>
    public partial class ShoppingCartUC : UserControl
    {
        private MainWindow mainWindow;
        private List<Product> InCart { get; set; }

        /// <summary>
        /// The constructor of ShoppingCartUC.
        /// </summary>
        /// <param name="mainWindow">
        /// MainWindow is used to send data to the server and change the view of the application.
        /// </param>
        public ShoppingCartUC(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();

            InCart = new List<Product>();

            InCartProductsList.ItemsSource = InCart;
        }

        /// <summary>
        /// Used to change the view to the Home screen.
        /// </summary>
        /// <param name="sender">
        /// The data of the object.
        /// </param>
        /// <param name="e">
        /// The button event argument.
        /// </param>
        private void Button_ContinueShopping(object sender, RoutedEventArgs e)
        {
            mainWindow.ChangeView("MainProduct");
        }

        /// <summary>
        /// Used to change the view of the application.
        /// </summary>
        /// <param name="sender">
        /// The data of the object.
        /// </param>
        /// <param name="e">
        /// The button event argument.
        /// </param>
        private void Button_ProceedPurchase(object sender, RoutedEventArgs e)
        {
            mainWindow.ChangeView("PurchaseCheckout");
        }

        /// <summary>
        /// Button to remove a product from the users cart.
        /// </summary>
        /// <param name="sender">
        /// The data of the object.
        /// </param>
        /// <param name="e">
        /// The button event argument.
        /// </param>
        private void Button_Remove(object sender, RoutedEventArgs e)
        {

            string p = ((Button)sender).Tag.ToString();

            foreach (Product product in InCart)
            {
                if (product.Name.Equals(p))
                {
                    mainWindow.RemoveFromCart(product);
                }
            }
        }

        /// <summary>
        /// Used to set the displayed list of products.
        /// </summary>
        /// <param name="products">
        /// The list of products to be displayed on the screen.
        /// </param>
        public void SetInCart(List<Product> products)
        {
            InCartProductsList.ItemsSource = new List<Product>();
            InCartProductsList.ItemsSource = products;
        }
    }
}
