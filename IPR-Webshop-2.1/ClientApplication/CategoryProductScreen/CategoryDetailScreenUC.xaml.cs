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

namespace ClientApplication.CategoryProductScreen
{
    /// <summary>
    /// Interaction logic for CategoryDetailScreenUC.xaml
    /// </summary>
    public partial class CategoryDetailScreenUC : UserControl
    {
        private MainWindow mainWindow;
        private List<Product> CurrentProducts { get; set; }

        public CategoryDetailScreenUC(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();

            CategoryListBox.ItemsSource = mainWindow.ListViewProducts;
        }

        public void SetListView(string category)
        {
            foreach (ListViewSelectProduct LVP in mainWindow.ListViewProducts)
            {
                if (LVP.SelectId.Equals(category))
                {
                    CurrentProducts = LVP.Products;
                    ListViewProducts.ItemsSource = CurrentProducts;

                    CategoryTitle.Content = category;
                    return;
                }
            }
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item.IsSelected)
            {
                if (item != null)
                {
                    CurrentProducts = ((ListViewSelectProduct)item.DataContext).Products;
                    ListViewProducts.ItemsSource = CurrentProducts;
                    CategoryTitle.Content = ((ListViewSelectProduct)item.DataContext).SelectId;
                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (Product p in CurrentProducts)
            {
                if (((Button)sender).Tag.ToString().Equals(p.Name))
                {

                    mainWindow.SetProductDetail(p);
                    mainWindow.ChangeView("ProductDetail");
                }
            }
        }
    }
}
