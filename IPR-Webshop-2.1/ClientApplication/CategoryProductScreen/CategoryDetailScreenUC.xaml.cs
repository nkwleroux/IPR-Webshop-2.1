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

            var categories = GetCategoryProducts();
            if (categories.Count > 0)
                CategoryListBox.ItemsSource = categories;
        }

        private List<ListViewSelectProduct> GetCategoryProducts()
        {
            return new List<ListViewSelectProduct>()
            {
                new ListViewSelectProduct("Aardappel, groente, fruit", new List<Category>(){
                        new Category("Aardappel, groente, fruit","/Assets/images/aardappelen_groente_fruit.png")
                }),
                new ListViewSelectProduct("Salades, pizza, maaltijden", new List<Category>(){
                       new Category("Salades, pizza, maaltijden","/Assets/images/salades-pizza-maaltijden.png")
                }),
                new ListViewSelectProduct("Vlees, kip, vis, vega", new List<Category>(){
                    new Category("Vlees, kip, vis, vega","/Assets/images/vlees-kip-vis-vega.png")
                }),
                new ListViewSelectProduct("Kaas, vleeswaren, tapas", new List<Category>(){
                    new Category("Kaas, vleeswaren, tapas","/Assets/images/kaas-vleeswaren-tapas.png"),
                }),
                new ListViewSelectProduct("Zuivel, plantaardig en eiren", new List<Category>(){
                    new Category("Zuivel, plantaardig en eiren","/Assets/images/boter-eieren-zuivel.png")
                }),
                new ListViewSelectProduct("Bakkerij en banket", new List<Category>(){
                    new Category("Bakkerij en banket","/Assets/images/brood-gebak.png")
                }),
                new ListViewSelectProduct("Ontbijtgranen, beleg, tussendoor", new List<Category>(){
                    new Category("Ontbijtgranen, beleg, tussendoor","/Assets/images/ontbijtgranen-beleg-tussendoor.png")
                }),
                new ListViewSelectProduct("Frisdrank, sappen, koffie, thee", new List<Category>(){
                    new Category("Frisdrank, sappen, koffie, thee","/Assets/images/frisdrank-koffie-thee-sappen.png")
                }),
                new ListViewSelectProduct("Wijn en bubbels", new List<Category>(){
                    new Category("Wijn en bubbels","/Assets/images/wijn.png")
                }),
                new ListViewSelectProduct("Bier, sterke drank, aperitieven", new List<Category>(){
                    new Category("Bier, sterke drank, aperitieven","/Assets/images/bier-sterke-drank.png")
                }),
                new ListViewSelectProduct("Pasta, rijst en wereldkeuken", new List<Category>(){
                    new Category("Pasta, rijst en wereldkeuken","/Assets/images/pasta-rijst-wereldkeuken.png")
                }),
                new ListViewSelectProduct("Soepen, sauzen, kruiden, olie", new List<Category>(){
                    new Category("Soepen, sauzen, kruiden, olie","/Assets/images/soepen-sauzen-kruiden-olie.png")
                }),
                new ListViewSelectProduct("Snoep, koek, chips en chocolade", new List<Category>(){
                    new Category("Snoep, koek, chips en chocolade","/Assets/images/chips-koek-snoep-chocolade.png")
                }),
                new ListViewSelectProduct("Diepvries", new List<Category>(){
                    new Category("Diepvries","/Assets/images/diepvries.png")
                }),
                new ListViewSelectProduct("Baby, verzorging en hygiene", new List<Category>(){
                    new Category("Baby, verzorging en hygiene","/Assets/images/baby-drogisterij.png")
                }),
                new ListViewSelectProduct("Bewuste voeding", new List<Category>(){
                    new Category("Bewuste voeding","/Assets/images/bewuste-voeding.png")
                }),
                new ListViewSelectProduct("Huishouden, huisdier", new List<Category>(){
                    new Category("Huishouden, huisdier","/Assets/images/huishouden-huisdier.png")
                }),
                new ListViewSelectProduct("Koken, tafelen, vrije tijd", new List<Category>(){
                    new Category("Koken, tafelen, vrije tijd","/Assets/images/koken-tafelen-nonfood.png")
                })
            };
        }

        public void SetListView(string category)
        {
            foreach (ListViewSelectProduct LVP in GetCategoryProducts())
            {
                if (category.Equals(LVP.SelectId))
                {
                    ListViewProducts.ItemsSource = LVP.Categories;
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
                ListViewProducts.ItemsSource = ((ListViewSelectProduct)item.DataContext).Categories;
                CategoryTitle.Content = ((ListViewSelectProduct)item.DataContext).SelectId;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.ChangeView("ProductDetail");
        }
    }
}
