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
        public Page_ProductEditor(NetworkStream stream)
        {
            InitializeComponent();
            this.cachedImage = null;
            this.textBoxes = new TextBox[] { this.TextBox_ProductName, this.TextBox_ProductPrice, this.TextBox_ProductAmount };
            this.crypto = new Crypto(stream, HandleData);
            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage("client/productListRequest", getProductListRequest()));
        }
        private dynamic getProductListRequest()
        {
            return new
            {
                // TODO category
            };
        }
        private dynamic getProductChangeDynamic(string typeOfChange, Product p)
        {
            ProductSerializable product = DataProtocol.MakeSerializableProduct(p);
            return new
            {
                typeOfChange,
                product
            };
        }
        private dynamic getUserChangeDynamic(string typeOfChange, User user)
        {
            return new
            {
                typeOfChange,
                user
            };
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

            if(cachedImage != null && changed)
            {
                product.Image = cachedImage;
            }
            changed = false;
            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage(
                                            "client/productListChangeRequest", 
                                            getProductChangeDynamic("add", product)));
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

                if(cachedImage != null && changed)
                {
                    selectedProduct.Image = this.cachedImage;
                }
                changed = false;
                this.crypto.WriteTextMessage(DataProtocol.getJsonMessage(
                                            "client/productListChangeRequest",
                                            getProductChangeDynamic("edit", selectedProduct)));
            }
        }
        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            foreach(TextBox box in this.textBoxes)
            {
                box.Clear();
            }
        }
        private void Button_RemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            Product product = (Product)this.ListView_ProductList.SelectedItem;
            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage(
                                            "client/productListChangeRequest",
                                            getProductChangeDynamic("remove", product)));
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
        private BitmapImage cachedImage;
        private bool changed;
        private void Dialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            OpenFileDialog dialog = (OpenFileDialog)sender;
            cachedImage = new BitmapImage(new Uri(dialog.FileName));
            this.Image_ProductImage.Source = cachedImage;
            changed = true;
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
                    this.Image_ProductImage.Source = selectedObject.Image;
                    this.cachedImage = selectedObject.Image;
                }
            }
        }
        #endregion

        private void HandleData(string receivedText)
        {
            JObject receivedMessage = (JObject)JsonConvert.DeserializeObject(receivedText);
            // Type of message received.
            string type = (string)receivedMessage["type"];
            JObject receivedData = (JObject)receivedMessage["data"];

            switch (type)
            {
                case "server/productListResponse":
                    this.handleProductList(receivedData);
                    break;
                default:
                    // TODO: when message is not undestood.
                    Debug.WriteLine($"received type: {type}");
                    return;
            }
        }
        private void handleProductList(JObject data)
        {
            this.Dispatcher.Invoke(() => this.ListView_ProductList.Items.Clear());
            JArray productList = (JArray)data["productList"];
            foreach(JToken product in productList)
            {
                ProductSerializable serializable = JsonConvert.DeserializeObject<ProductSerializable>(product.ToString());
                Product productFromList = new Product(serializable);
                this.Dispatcher.Invoke(() => this.ListView_ProductList.Items.Add(productFromList));
            }
        }
        public void disconnectMessage()
        {
            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage("client/disconnect", new { }));
        }
    }
}
