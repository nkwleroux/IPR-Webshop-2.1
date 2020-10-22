using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
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

namespace ServerEditor.Interface_pages
{
    /// <summary>
    /// Interaction logic for Page_ProductEditor.xaml
    /// </summary>
    public partial class Page_ProductEditor : Page
    {
        private TextBox[] textBoxes;
        private Crypto crypto;
        public Page_ProductEditor(Crypto crypto)
        {
            InitializeComponent();
            this.crypto = crypto;
            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage("client/productListRequest",
                DataProtocol.getProductListRequest("TODO")));

            this.cachedImage = null;
            this.textBoxes = new TextBox[] { this.TextBox_ProductName, this.TextBox_ProductPrice, this.TextBox_ProductAmount };
        }

        #region Callbacks
        private void Button_AddProduct_Click(object sender, RoutedEventArgs e)
        {
            foreach (TextBox box in textBoxes)
            {
                if (box.Text.Length <= 0)
                    return;
            }

            double price;
            int amount;
            if (!double.TryParse(this.TextBox_ProductPrice.Text, out price) ||
                !int.TryParse(this.TextBox_ProductAmount.Text, out amount))
                return;

            Product product = new Product()
            {
                Name = this.TextBox_ProductName.Text,
                Price = price,
                Amount = amount,
            };

            if (cachedImage != null)
            {
                product.Image = cachedImage;
            }

            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage(
                                            "client/productListChangeRequest",
                                            DataProtocol.getProductChangeDynamic("add", product)));
        }
        private void Button_EditProduct_Click(object sender, RoutedEventArgs e)
        {
            Product selectedProduct = (Product)this.ListView_ProductList.SelectedItem;
            if (selectedProduct != null)
            {
                foreach (TextBox box in textBoxes)
                {
                    if (box.Text.Length <= 0)
                        return;
                }

                double price;
                int amount;
                if (!double.TryParse(this.TextBox_ProductPrice.Text, out price) ||
                    !int.TryParse(this.TextBox_ProductAmount.Text, out amount))
                    return;

                selectedProduct.Name = this.TextBox_ProductName.Text;
                selectedProduct.Price = price;
                selectedProduct.Amount = amount;

                if (cachedImage != null)
                {
                    selectedProduct.Image = cachedImage;
                }
                this.crypto.WriteTextMessage(DataProtocol.getJsonMessage(
                                            "client/productListChangeRequest",
                                            DataProtocol.getProductChangeDynamic("edit", selectedProduct)));
            }
        }
        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            foreach (TextBox box in this.textBoxes)
            {
                box.Clear();
            }
            this.cachedImage = new byte[0];
            this.Dispatcher.Invoke(() => this.Image_ProductImage.Source = new BitmapImage());
        }
        private void Button_RemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            Product product = (Product)this.ListView_ProductList.SelectedItem;
            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage(
                                            "client/productListChangeRequest",
                                            DataProtocol.getProductChangeDynamic("remove", product)));
        }
        private void Button_ChangeImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter =
               "png files (*.png)|*.png|jpeg files (*.jpeg)|*.jpeg|jpg files (*.jpg)|*.jpg";
            dialog.InitialDirectory = "C:\\";
            dialog.Title = "Select an image file";
            dialog.FileOk += Dialog_FileOk;
            dialog.ShowDialog();
        }
        private byte[] cachedImage;
        private void Dialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            OpenFileDialog dialog = (OpenFileDialog)sender;
            BitmapImage bitmapImage = new BitmapImage(new Uri(dialog.FileName));
            this.Dispatcher.Invoke(() => this.Image_ProductImage.Source = bitmapImage);
            this.cachedImage = BitmapConverter.ConvertImageToByteArray(bitmapImage);
        }
        private void MyListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListView list = (ListView)sender;
            if (list.Items.Count > 0)
            {
                Product selectedObject = (Product)list.SelectedItem;
                if (selectedObject != null)
                {
                    this.TextBox_ProductName.Text = selectedObject.Name;
                    this.TextBox_ProductPrice.Text = selectedObject.Price.ToString();
                    this.TextBox_ProductAmount.Text = selectedObject.Amount.ToString();

                    BitmapImage image = BitmapConverter.LoadImage(selectedObject.Image);
                    this.Dispatcher.Invoke(() => this.Image_ProductImage.Source = image);
                }
            }
        }
        #endregion

        public void handleProductList(JObject receivedData)
        {
            this.Dispatcher.Invoke(() => this.ListView_ProductList.Items.Clear());
            JArray productList = (JArray)receivedData["productList"];
            foreach (JToken Jproduct in productList)
            {
                Product product = JsonConvert.DeserializeObject<Product>(Jproduct.ToString());
                this.Dispatcher.Invoke(() => this.ListView_ProductList.Items.Add(product));
            }
        }
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListViewItem item = (ListViewItem)sender;
            Product selectedObject = (Product)item.DataContext;

            if (selectedObject != null)
            {
                this.TextBox_ProductName.Text = selectedObject.Name;
                this.TextBox_ProductPrice.Text = selectedObject.Price.ToString();
                this.TextBox_ProductAmount.Text = selectedObject.Amount.ToString();

                BitmapImage image = BitmapConverter.LoadImage(selectedObject.Image);
                this.Dispatcher.Invoke(() => this.Image_ProductImage.Source = image);
            }

        }
    }
}
