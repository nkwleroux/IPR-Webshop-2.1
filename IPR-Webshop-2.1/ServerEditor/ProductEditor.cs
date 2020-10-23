using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServerEditor.Interface_pages;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ServerEditor
{
    public class ProductEditor
    {
        private Page_ProductEditor viewModel;
        private Crypto crypto;
        private byte[] cachedImage;
        public ProductEditor(Crypto crypto, Page_ProductEditor page)
        {
            this.cachedImage = null;
            this.viewModel = page;
            this.crypto = crypto;
            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage("client/productListRequest",
                DataProtocol.getProductListRequest("")));
        }
        public void OnAdd()
        {
            foreach (TextBox box in viewModel.textBoxes)
            {
                if (box.Text.Length <= 0)
                    return;
            }

            double price;
            int amount;
            if (!double.TryParse(viewModel.TextBox_ProductPrice.Text, out price) ||
                !int.TryParse(viewModel.TextBox_ProductAmount.Text, out amount))
                return;

            Product product = new Product()
            {
                Name = viewModel.TextBox_ProductName.Text,
                Category = viewModel.ComboBox_category.Text,
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
        public void OnEdit()
        {
            Product selectedProduct = (Product)viewModel.ListView_ProductList.SelectedItem;
            if (selectedProduct != null)
            {
                foreach (TextBox box in viewModel.textBoxes)
                {
                    if (box.Text.Length <= 0)
                        return;
                }

                double price;
                int amount;
                if (!double.TryParse(viewModel.TextBox_ProductPrice.Text, out price) ||
                    !int.TryParse(viewModel.TextBox_ProductAmount.Text, out amount))
                    return;

                selectedProduct.Name = viewModel.TextBox_ProductName.Text;
                selectedProduct.Category = viewModel.ComboBox_category.Text;
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
        public void OnRemove()
        {
            Product product = (Product)viewModel.ListView_ProductList.SelectedItem;
            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage(
                                            "client/productListChangeRequest",
                                            DataProtocol.getProductChangeDynamic("remove", product)));
        }
        public void OnClear()
        {
            this.cachedImage = new byte[0];
            viewModel.Dispatcher.Invoke(() => viewModel.Image_ProductImage.Source = new BitmapImage());
        }
        public void ImageSelected(string fileName)
        {
            BitmapImage bitmapImage = new BitmapImage(new Uri(fileName));
            viewModel.Dispatcher.Invoke(() => viewModel.Image_ProductImage.Source = bitmapImage);
            this.cachedImage = BitmapConverter.ConvertImageToByteArray(bitmapImage);
        }
        public void OnItemClick(ListViewItem item)
        {
            Product selectedObject = (Product)item.DataContext;

            if (selectedObject != null)
            {
                viewModel.TextBox_ProductName.Text = selectedObject.Name;
                viewModel.TextBox_ProductPrice.Text = selectedObject.Price.ToString();
                viewModel.TextBox_ProductAmount.Text = selectedObject.Amount.ToString();
                viewModel.ComboBox_category.SelectedItem = selectedObject.Category;
                BitmapImage image = BitmapConverter.LoadImage(selectedObject.Image);
                viewModel.Dispatcher.Invoke(() => viewModel.Image_ProductImage.Source = image);
            }
        }
        public void OnProductListReceived(JObject receivedData)
        {
            viewModel.Dispatcher.Invoke(() => viewModel.ListView_ProductList.Items.Clear());
            JArray productList = (JArray)receivedData["productList"];
            foreach (JToken Jproduct in productList)
            {
                Product product = JsonConvert.DeserializeObject<Product>(Jproduct.ToString());
                viewModel.Dispatcher.Invoke(() => viewModel.ListView_ProductList.Items.Add(product));
            }
        }
    }
}
