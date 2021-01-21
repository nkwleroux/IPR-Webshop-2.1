using Microsoft.Win32;
using Shared;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ServerEditor.Interface_pages
{
    /// <summary>
    /// Interaction logic for Page_ProductEditor.xaml
    /// </summary>
    public partial class Page_ProductEditor : Page
    {
        public TextBox[] textBoxes;
        public ProductEditor ProductEditor;
        public Page_ProductEditor(Crypto crypto)
        {
            InitializeComponent();
            List<string> categories = new List<string>();
            categories.AddRange(new string[] { "Aardappel, groente, fruit",
                "Salades, pizza, maaltijden",
                "Vlees, kip, vis, vega",
                "Kaas, vleeswaren, tapas",
                "Zuivel, plantaardig en eiren",
                "Bakkerij en banket",
                "Ontbijtgranen, beleg, tussendoor",
                "Frisdrank, sappen, koffie, thee",
                "Wijn en bubbels",
                "Bier, sterke drank, aperitieven",
                "Pasta, rijst en wereldkeuken",
                "Soepen, sauzen, kruiden, olie",
                "Snoep, koek, chips en chocolade",
                "Diepvries",
                "Baby, verzorging en hygiene",
                "Bewuste voeding",
                "Huishouden, huisdier",
                "Koken, tafelen, vrije tijd"});

            this.ComboBox_category.ItemsSource = categories;
            this.ComboBox_category.SelectedIndex = 0;

            this.textBoxes = new TextBox[] {
                this.TextBox_ProductName,
                this.TextBox_ProductPrice,
                this.TextBox_ProductAmount };

            this.ProductEditor = new ProductEditor(crypto, this);
        }

        #region Callbacks
        private void Button_AddProduct_Click(object sender, RoutedEventArgs e)
        {
            this.ProductEditor.OnAdd();
        }
        private void Button_EditProduct_Click(object sender, RoutedEventArgs e)
        {
            this.ProductEditor.OnEdit();
        }
        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            foreach (TextBox box in this.textBoxes)
            {
                box.Clear();
            }
            this.ProductEditor.OnClear();
        }
        private void Button_RemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            this.ProductEditor.OnRemove();
        }
        private void Button_ChangeImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            // adds a filter with a list of usable image filetypes.
            dialog.Filter =
               "png files (*.png)|*.png|jpeg files (*.jpeg)|*.jpeg|jpg files (*.jpg)|*.jpg";
            dialog.InitialDirectory = "C:\\";
            dialog.Title = "Select an image file";
            dialog.FileOk += Dialog_FileOk;
            // shows a dialog where a file can be picked.
            dialog.ShowDialog();
        }
        private void Dialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            OpenFileDialog dialog = (OpenFileDialog)sender;
            this.ProductEditor.ImageSelected(dialog.FileName);
        }
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListViewItem item = (ListViewItem)sender;
            this.ProductEditor.OnItemClick(item);
        }
        #endregion
    }
}
