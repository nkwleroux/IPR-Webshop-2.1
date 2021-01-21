using Shared;
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

namespace Shared.ProductDetailScreen
{
    /// <summary>
    /// Interaction logic for ProductDetailScreenUC.xaml
    /// </summary>
    public partial class ProductDetailScreenUC : UserControl
    {
        private MainWindow mainWindow;
        private Product product;

        /// <summary>
        /// The constructor of ProductDetailScreenUC
        /// </summary>
        /// <param name="mainWindow">
        /// MainWindow is used to send data to the server.
        /// </param>
        public ProductDetailScreenUC(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
        }

        /// <summary>
        /// Used to set the name of the product and the amount of products that the user can select to put in the cart.
        /// </summary>
        /// <param name="p">
        /// The product which contains the data for the name and total amount of items of that product.
        /// </param>
        public void SetProductDetail(Product p)
        {
            product = p;

            ImageBrush_Image.ImageSource = p.bitmapImage;
            TextBlock_Name.Text = p.Name;
            if (p.Amount > 0)
            {
                TextBlock_Stock.Text = p.Amount.ToString();
            }
            else
            {
                TextBlock_Stock.Text = "None";
            }

            if (p.Amount >= 1)
            {
                ComboBox_Quantity.ItemsSource = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 15, 20, 25, 50 };
            }
            else
            {
                ComboBox_Quantity.ItemsSource = new int[] { 0 };
            }

            ComboBox_Quantity.SelectedIndex = 0;
        }

        /// <summary>
        /// Used to add a product to the users cart.
        /// </summary>
        /// <param name="sender">
        /// The data of the object.
        /// </param>
        /// <param name="e">
        /// The button event argument.
        /// </param>
        private void Button_AddToCart(object sender, RoutedEventArgs e)
        {
            Product product = new Product(this.product);
            product.Amount = int.Parse(ComboBox_Quantity.SelectedItem.ToString());
            product.Price *= product.Amount;
            mainWindow.AddToCart(product);
        }
    }
}
