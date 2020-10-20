using Microsoft.Win32;
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

namespace ServerEditor.Interface_pages
{
    /// <summary>
    /// Interaction logic for Page_ProductEditor.xaml
    /// </summary>
    public partial class Page_ProductEditor : Page
    {
        private TextBox[] textBoxes;
        public Page_ProductEditor()
        {
            InitializeComponent();
            this.textBoxes = new TextBox[] { this.TextBox_ProductName, this.TextBox_ProductPrice, this.TextBox_ProductAmount };
            this.ListView_ProductList.Items.Add(new Product()
            {
                Name = "Pindakaas",
                Price = 2.45,
                Amount = 10345
            });
        }

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

            this.ListView_ProductList.Items.Add(new Product()
            {
                Name = this.TextBox_ProductName.Text,
                Price = price,
                Amount = amount
            });
        }

        private void Button_EditProduct_Click(object sender, RoutedEventArgs e)
        {
            Product selectedProduct = (Product)this.ListView_ProductList.SelectedItem;
            int index = this.ListView_ProductList.Items.IndexOf(selectedProduct);
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
                this.ListView_ProductList.Items.Remove(selectedProduct);
                this.ListView_ProductList.Items.Insert(index, selectedProduct);
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
            this.ListView_ProductList.Items.Remove(this.ListView_ProductList.SelectedItem);
        }

        private void Button_ChangeImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter =
               "png files (*.png)|*.png|jpeg files (*.jpeg)|*.jpeg";
            dialog.InitialDirectory = "C:\\";
            dialog.Title = "Select an image file";
            dialog.FileOk += Dialog_FileOk;
            dialog.ShowDialog();
        }

        private void Dialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Product selectedProduct = (Product)this.ListView_ProductList.SelectedItem;
            OpenFileDialog dialog = (OpenFileDialog)sender;
            selectedProduct.Image = new BitmapImage(new Uri(dialog.FileName));
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
                }
            }
        }
    }
}
