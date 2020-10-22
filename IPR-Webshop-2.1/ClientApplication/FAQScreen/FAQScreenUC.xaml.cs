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

namespace ClientApplication.FAQScreen
{
    /// <summary>
    /// Interaction logic for FAQScreenUC.xaml
    /// </summary>
    public partial class FAQScreenUC : UserControl
    {
        private MainWindow mainWindow;

        public FAQScreenUC(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();

            var data = GetFAQData();
            QuestionsListBox.ItemsSource = data;
        }

        private List<FAQData> GetFAQData()
        {

            return new List<FAQData>()
            {
                new FAQData("How to find a product","To find a product, click a category"),
                new FAQData("Test","To find a product, click a ca23423tegory"),
                new FAQData("Test2","To find a product, click aasdasd category"),
                new FAQData("Test3","To find a product, click a category"),
                new FAQData("Test4","To find a product, click absrbt category")
                };
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                AnswerBox.Text = ((FAQData)item.DataContext).Answer;
            }

        }
    }
}
