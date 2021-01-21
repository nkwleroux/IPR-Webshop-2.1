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

namespace Shared.CategoryProductScreen
{
    /// <summary>
    /// Interaction logic for CategoryDetailScreenUC.xaml
    /// </summary>
    public partial class CategoryDetailScreenUC : UserControl
    {
        private MainWindow mainWindow;
        private List<Product> CurrentProducts { get; set; }

        /// <summary>
        /// The constructor of CategoryDetailScreenUC.
        /// </summary>
        /// <param name="mainWindow">
        /// MainWindow is used to send data to the server and change the view of the application.
        /// </param>
        public CategoryDetailScreenUC(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();

            CategoryListBox.ItemsSource = mainWindow.ListViewProducts;
        }

        /// <summary>
        /// Used to set the currently displayed list on the view.
        /// </summary>
        /// <param name="category">
        /// The category to which the data of the list will be changed to.
        /// </param>
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

        /// <summary>
        /// The onClick method. Used to check if there has been a left click action on a list item.
        /// </summary>
        /// <param name="sender">
        /// The data of the object.
        /// </param>
        /// <param name="e">
        /// The button event argument.
        /// </param>
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

        /// <summary>
        /// The onClick method. Used to change the view to the selected item.
        /// </summary>
        /// <param name="sender">
        /// The data of the object.
        /// </param>
        /// <param name="e">
        /// The button event argument.
        /// </param>
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
