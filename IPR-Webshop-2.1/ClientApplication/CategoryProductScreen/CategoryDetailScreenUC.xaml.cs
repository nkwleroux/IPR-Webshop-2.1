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

        public CategoryDetailScreenUC(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();

            var categories = mainWindow.ListViewProducts;
            if (categories.Count > 0)
                CategoryListBox.ItemsSource = categories;
        }

        public void SetListView(string category)
        {
            foreach (ListViewSelectProduct LVP in mainWindow.ListViewProducts)
            {
                if (category.Equals(LVP.SelectId))
                {
                    ListViewProducts.ItemsSource = LVP.Products;
                                       
                    CategoryTitle.Content = category;
                    return;
                }
            }
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                ListViewProducts.ItemsSource = ((ListViewSelectProduct)item.DataContext).Products;
                CategoryTitle.Content = ((ListViewSelectProduct)item.DataContext).SelectId;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.ChangeView("ProductDetail");
        }
    }
}
